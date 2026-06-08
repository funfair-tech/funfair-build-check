using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.Runner;
using FunFair.BuildCheck.Runner.Services;
using FunFair.BuildCheck.Runner.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.Runner.Tests.Services;

public sealed class TrackingLoggerTests : TestBase
{
    [Fact]
    public Task ErrorsStartsAtZeroAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) => Assert.Equal(expected: 0, actual: logger.Errors)
        );

    [Fact]
    public Task IsErroredIsFalseWhenNoErrorsLoggedAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) =>
                Assert.False(logger.IsErrored, userMessage: "IsErrored should be false when no errors have been logged")
        );

    [Fact]
    public Task IsErroredIsTrueAfterErrorIsLoggedAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) =>
            {
                logger.Log(LogLevel.Error, default, "state", null, (s, e) => s);
                Assert.True(logger.IsErrored, userMessage: "IsErrored should be true after an error has been logged");
            }
        );

    [Fact]
    public Task LogWithDebugLevelDoesNotForwardToInnerLoggerAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, inner) =>
            {
                logger.Log(LogLevel.Debug, default, "state", null, (s, e) => s);
                Assert.Empty(inner.Levels);
            }
        );

    [Fact]
    public Task LogWithWarningWhenWarningsAsErrorsTreatsWarningAsErrorAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: true,
            (logger, inner) =>
            {
                logger.Log(LogLevel.Warning, default, "state", null, (s, e) => s);
                Assert.Equal(expected: 1, actual: logger.Errors);
                Assert.Contains(LogLevel.Error, inner.Levels);
            }
        );

    [Theory]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Critical)]
    public Task LogWithErrorOrCriticalLevelIncrementsErrorsCountAsync(LogLevel logLevel) =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) =>
            {
                logger.Log(logLevel, default, "state", null, (s, e) => s);
                Assert.Equal(expected: 1, actual: logger.Errors);
            }
        );

    [Fact]
    public Task IsEnabledReturnsFalseForDebugLevelAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) =>
                Assert.False(logger.IsEnabled(LogLevel.Debug), userMessage: "Debug log level should be disabled")
        );

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Critical)]
    public Task IsEnabledReturnsTrueForNonDebugLogLevelsAsync(LogLevel logLevel) =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) => Assert.True(logger.IsEnabled(logLevel), userMessage: "Non-debug log level should be enabled")
        );

    [Fact]
    public Task BeginScopeReturnedScopeCanBeDisposedAsync() =>
        this.WithTrackingLoggerAsync(
            warningsAsErrors: false,
            (logger, _) =>
            {
                using IDisposable? scope = logger.BeginScope("state");
                Assert.NotNull(scope);
            }
        );

    private async Task WithTrackingLoggerAsync(bool warningsAsErrors, Action<ITrackingLogger, CapturingLogger> action)
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnxPath = Path.Combine(tempDir, "test.slnx");
            await File.WriteAllTextAsync(slnxPath, "<Solution></Solution>", this.CancellationToken());

            IFrameworkSettings frameworkSettings = GetSubstitute<IFrameworkSettings>();
            frameworkSettings.ProjectImport.Returns(string.Empty);
            frameworkSettings.DotNetAllowPreReleaseSdk.Returns(string.Empty);
            IProjectClassifier projectClassifier = GetSubstitute<IProjectClassifier>();
            CapturingLogger inner = new();
            ITrackingLogger? capturedLogger = null;

            await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: warningsAsErrors,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: new CheckConfiguration(PreReleaseBuild: false, AllowPackageVersionMismatch: false),
                buildServiceProvider: CaptureAndBuild,
                logger: inner,
                cancellationToken: this.CancellationToken()
            );

            Assert.NotNull(capturedLogger);
            action(capturedLogger, inner);

            return;

            IServiceProvider CaptureAndBuild(IServiceCollection services)
            {
                ServiceDescriptor[] toRemove =
                [
                    .. services.Where(s =>
                        s.ServiceType == typeof(ISolutionCheck) || s.ServiceType == typeof(IProjectCheck)
                    ),
                ];

                foreach (ServiceDescriptor descriptor in toRemove)
                {
                    services.Remove(descriptor);
                }

                IServiceProvider sp = services.BuildServiceProvider();
                capturedLogger = sp.GetRequiredService<ITrackingLogger>();

                return sp;
            }
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
