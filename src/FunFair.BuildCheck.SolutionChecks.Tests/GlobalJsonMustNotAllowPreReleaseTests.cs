using System;
using System.IO;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.SolutionChecks.Tests;

public sealed class GlobalJsonMustNotAllowPreReleaseTests : TestBase
{
    [Fact]
    public async Task WhenNoDotNetSdkVersionConfiguredSkipsCheckAsync()
    {
        IRepositorySettings settings = Substitute.For<IRepositorySettings>();
        settings.DotNetSdkVersion.Returns((string?)null);
        settings.DotNetAllowPreReleaseSdk.Returns("false");

        CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
        GlobalJsonMustNotAllowPreRelease check = new(
            repositorySettings: settings,
            loader: new GlobalJsonLoader(),
            logger: logger
        );

        await check.CheckAsync(solutionFileName: "/some/solution.slnx", cancellationToken: this.CancellationToken());

        Assert.Empty(logger.Entries);
    }

    [Fact]
    public async Task WhenGlobalJsonIsMissingSkipsCheckAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("false");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.Empty(logger.Entries);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task WhenAllowPrereleaseNotSpecifiedErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, allowPrerelease: null);

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("false");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task WhenAllowPrereleaseFalseAndConfigDisallowsNoErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, allowPrerelease: false);

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("false");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task WhenAllowPrereleaseTrueButConfigDisallowsErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, allowPrerelease: true);

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("false");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task WhenAllowPrereleaseTrueAndConfigAllowsNoErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, allowPrerelease: true);

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("true");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task WhenGlobalJsonIsInvalidErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await File.WriteAllTextAsync(
                path: Path.Combine(path1: tempDir, path2: "global.json"),
                contents: "{ invalid json }",
                cancellationToken: this.CancellationToken()
            );

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");
            settings.DotNetAllowPreReleaseSdk.Returns("false");

            CapturingLogger<GlobalJsonMustNotAllowPreRelease> logger = new();
            GlobalJsonMustNotAllowPreRelease check = new(
                repositorySettings: settings,
                loader: new GlobalJsonLoader(),
                logger: logger
            );

            await check.CheckAsync(
                solutionFileName: Path.Combine(path1: tempDir, path2: "Solution.slnx"),
                cancellationToken: this.CancellationToken()
            );

            Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
        }
        finally
        {
            Directory.Delete(path: tempDir, recursive: true);
        }
    }

    private static string CreateTempDirectory()
    {
        string path = Path.Combine(path1: Path.GetTempPath(), path2: Path.GetRandomFileName());
        Directory.CreateDirectory(path);

        return path;
    }

    private Task WriteGlobalJsonAsync(string tempDir, bool? allowPrerelease)
    {
        if (!allowPrerelease.HasValue)
        {
            return File.WriteAllTextAsync(
                path: Path.Combine(path1: tempDir, path2: "global.json"),
                contents: "{\"sdk\":{\"version\":\"9.0.100\"}}",
                cancellationToken: this.CancellationToken()
            );
        }

        string preReleaseValue = allowPrerelease.Value ? "true" : "false";
        string content = $"{{\"sdk\":{{\"version\":\"9.0.100\",\"allowPrerelease\":{preReleaseValue}}}}}";

        return File.WriteAllTextAsync(
            path: Path.Combine(path1: tempDir, path2: "global.json"),
            contents: content,
            cancellationToken: this.CancellationToken()
        );
    }
}
