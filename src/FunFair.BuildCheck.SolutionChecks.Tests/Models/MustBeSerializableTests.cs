using System;
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
}
