using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using FunFair.BuildCheck.SolutionChecks.Models;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.SolutionChecks.Tests.Models;

public sealed class MustBeSerializableTests : TestBase
{
    [Fact]
    public void DefaultContextRegistersAllExpectedTypes()
    {
        MustBeSerializable context = MustBeSerializable.Default;

        JsonTypeInfo<GlobalJsonPacket> packetInfo = context.GlobalJsonPacket;
        Assert.NotNull(packetInfo);

        JsonTypeInfo<GlobalJsonSdk> sdkInfo = context.GlobalJsonSdk;
        Assert.NotNull(sdkInfo);

        JsonTypeInfo<string> stringInfo = context.String;
        Assert.NotNull(stringInfo);

        JsonTypeInfo<bool> boolInfo = context.Boolean;
        Assert.NotNull(boolInfo);

        JsonTypeInfo<bool?> nullableBoolInfo = context.NullableBoolean;
        Assert.NotNull(nullableBoolInfo);
    }

    [Fact]
    public void NewInstanceRegistersAllExpectedTypes()
    {
        MustBeSerializable context = new();

        JsonTypeInfo<GlobalJsonPacket> packetInfo = context.GlobalJsonPacket;
        Assert.NotNull(packetInfo);
    }

    [Fact]
    public void GetTypeInfoReturnsInfoForRegisteredType()
    {
        MustBeSerializable context = new();

        JsonTypeInfo? info = context.GetTypeInfo(typeof(GlobalJsonPacket));
        Assert.NotNull(info);
    }

    [Fact]
    public void GetTypeInfoReturnsNullForUnregisteredType()
    {
        MustBeSerializable context = new();

        JsonTypeInfo? info = context.GetTypeInfo(typeof(DateTime));
        Assert.Null(info);
    }

    [Fact]
    public void CanRoundTripGlobalJsonPacket()
    {
        GlobalJsonSdk sdk = new(version: "9.0.100", rollForward: "latestPatch", allowPrerelease: null);
        GlobalJsonPacket packet = new(sdk: sdk);

        string json = JsonSerializer.Serialize(packet, MustBeSerializable.Default.GlobalJsonPacket);
        GlobalJsonPacket? result = JsonSerializer.Deserialize(json, MustBeSerializable.Default.GlobalJsonPacket);

        Assert.NotNull(result);
        Assert.Equal(expected: "9.0.100", actual: result.Sdk?.Version);
        Assert.Equal(expected: "latestPatch", actual: result.Sdk?.RollForward);
    }

    [Fact]
    public void CanDeserializeGlobalJsonPacketWithNullSdk()
    {
        GlobalJsonPacket? result = JsonSerializer.Deserialize("{}", MustBeSerializable.Default.GlobalJsonPacket);

        Assert.NotNull(result);
        Assert.Null(result.Sdk);
    }

    [Fact]
    public void CanDeserializeGlobalJsonPacketSettingSdkViaProperty()
    {
        const string json = "{\"sdk\":{\"version\":\"9.0.100\",\"rollForward\":\"latestPatch\"}}";
        GlobalJsonPacket? result = JsonSerializer.Deserialize(json, MustBeSerializable.Default.GlobalJsonPacket);

        Assert.NotNull(result);
        Assert.Equal(expected: "9.0.100", actual: result.Sdk?.Version);
        Assert.Equal(expected: "latestPatch", actual: result.Sdk?.RollForward);
        Assert.Null(result.Sdk?.AllowPrerelease);
    }

    [Fact]
    public void ContextWithCustomStringConverterHandlesRuntimeConverter()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new UppercaseStringConverter());
        MustBeSerializable context = new(options);

        JsonTypeInfo<string> info = context.String;
        Assert.NotNull(info);
    }

    [Fact]
    public void ContextWithStringConverterFactoryHandlesExpandedConverter()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new StringConverterFactory());
        MustBeSerializable context = new(options);

        JsonTypeInfo<string> info = context.String;
        Assert.NotNull(info);
    }

    private sealed class UppercaseStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.GetString()?.ToUpperInvariant();

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToUpperInvariant());
    }

    private sealed class StringConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(string);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            new UppercaseStringConverter();
    }

    [Fact]
    public void CanSerializeNullGlobalJsonPacket()
    {
        string json = JsonSerializer.Serialize(value: null, jsonTypeInfo: MustBeSerializable.Default.GlobalJsonPacket);
        Assert.Equal(expected: "null", actual: json);
    }

    [Fact]
    public void CanSerializeNullGlobalJsonSdk()
    {
        string json = JsonSerializer.Serialize(value: null, jsonTypeInfo: MustBeSerializable.Default.GlobalJsonSdk);
        Assert.Equal(expected: "null", actual: json);
    }

    [Fact]
    public void CanRoundTripGlobalJsonSdkWithAllowPrereleaseTrue()
    {
        GlobalJsonSdk sdk = new(version: "9.0.100", rollForward: "latestPatch", allowPrerelease: true);
        string json = JsonSerializer.Serialize(value: sdk, jsonTypeInfo: MustBeSerializable.Default.GlobalJsonSdk);
        GlobalJsonSdk? result = JsonSerializer.Deserialize(
            json: json,
            jsonTypeInfo: MustBeSerializable.Default.GlobalJsonSdk
        );
        Assert.NotNull(result);
        Assert.True(result.AllowPrerelease);
    }

    [Fact]
    public void PropertyGetterDelegatesCanBeInvokedForGlobalJsonSdk()
    {
        GlobalJsonSdk sdk = new(version: "9.0.100", rollForward: "latestPatch", allowPrerelease: null);
        JsonTypeInfo<GlobalJsonSdk> typeInfo = MustBeSerializable.Default.GlobalJsonSdk;

        foreach (JsonPropertyInfo prop in typeInfo.Properties)
        {
            if (prop.Get is { } getter)
            {
                _ = getter(sdk);
            }
        }
    }

    [Fact]
    public void PropertyGetterDelegateCanBeInvokedForGlobalJsonPacket()
    {
        GlobalJsonPacket packet = new(sdk: null);
        JsonTypeInfo<GlobalJsonPacket> typeInfo = MustBeSerializable.Default.GlobalJsonPacket;

        foreach (JsonPropertyInfo prop in typeInfo.Properties)
        {
            if (prop.Get is { } getter)
            {
                _ = getter(packet);
            }
        }
    }

    [Fact]
    public void PropertySetterDelegateCanBeInvokedForGlobalJsonPacket()
    {
        GlobalJsonPacket packet = new(sdk: null);
        GlobalJsonSdk sdk = new(version: "9.0.100", rollForward: null, allowPrerelease: null);
        JsonTypeInfo<GlobalJsonPacket> typeInfo = MustBeSerializable.Default.GlobalJsonPacket;

        foreach (JsonPropertyInfo prop in typeInfo.Properties)
        {
            prop.Set?.Invoke(packet, sdk);
        }

        Assert.Same(expected: sdk, actual: packet.Sdk);
    }

    [Fact]
    public void PropertyAttributeProvidersCanBeAccessedForGlobalJsonPacket()
    {
        JsonTypeInfo<GlobalJsonPacket> typeInfo = MustBeSerializable.Default.GlobalJsonPacket;

        foreach (JsonPropertyInfo prop in typeInfo.Properties)
        {
            Assert.NotNull(prop.AttributeProvider);
        }
    }

    [Fact]
    public void PropertyAttributeProvidersCanBeAccessedForGlobalJsonSdk()
    {
        JsonTypeInfo<GlobalJsonSdk> typeInfo = MustBeSerializable.Default.GlobalJsonSdk;

        foreach (JsonPropertyInfo prop in typeInfo.Properties)
        {
            Assert.NotNull(prop.AttributeProvider);
        }
    }

    [Fact]
    public void ContextWithFactoryConverterReturningNullThrows()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new NullReturningStringConverterFactory());
        MustBeSerializable context = new(options);

        Assert.Throws<InvalidOperationException>(() => _ = context.String);
    }

    private sealed class NullReturningStringConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(string);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) => null;
    }
}
