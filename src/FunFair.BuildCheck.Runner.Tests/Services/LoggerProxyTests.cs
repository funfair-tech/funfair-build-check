using System;
using FunFair.BuildCheck.Runner.Services;
using FunFair.BuildCheck.Runner.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.Runner.Tests.Services;

public sealed class LoggerProxyTests : TestBase
{
    [Fact]
    public void LogDelegatesToInnerLogger()
    {
        CapturingLogger inner = new();
        LoggerProxy<LoggerProxyTests> proxy = new(inner);

        proxy.Log(
            logLevel: LogLevel.Information,
            eventId: default,
            state: "state",
            exception: null,
            formatter: (s, e) => s
        );

        Assert.Single(inner.Levels);
        Assert.Equal(expected: LogLevel.Information, actual: inner.Levels[0]);
    }

    [Fact]
    public void IsEnabledReturnsTrueWhenInnerReturnsTrue()
    {
        CapturingLogger inner = new(isEnabled: true);
        LoggerProxy<LoggerProxyTests> proxy = new(inner);

        bool result = proxy.IsEnabled(LogLevel.Information);

        Assert.True(result, "IsEnabled should return true when inner returns true");
    }

    [Fact]
    public void IsEnabledReturnsFalseWhenInnerReturnsFalse()
    {
        CapturingLogger inner = new(isEnabled: false);
        LoggerProxy<LoggerProxyTests> proxy = new(inner);

        bool result = proxy.IsEnabled(LogLevel.Debug);

        Assert.False(result, "IsEnabled should return false when inner returns false");
    }

    [Fact]
    public void BeginScopeDelegatesToInnerLogger()
    {
        CapturingLogger inner = new();
        LoggerProxy<LoggerProxyTests> proxy = new(inner);

        using IDisposable? scope = proxy.BeginScope("scope");

        Assert.True(inner.BeginScopeCalled, "BeginScope should have been called on inner logger");
    }
}
