using System;
using System.IO;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.SolutionChecks.Tests;

public sealed class GlobalJsonIsLatestTests : TestBase
{
    [Fact]
    public async Task WhenNoDotNetSdkVersionConfiguredSkipsCheckAsync()
    {
        IRepositorySettings settings = Substitute.For<IRepositorySettings>();
        settings.DotNetSdkVersion.Returns((string?)null);

        CapturingLogger<GlobalJsonIsLatest> logger = new();
        GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

        await check.CheckAsync(solutionFileName: "/some/solution.slnx", cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenSolutionFileHasNoParentDirectorySkipsCheckAsync()
    {
        IRepositorySettings settings = GetSubstitute<IRepositorySettings>();
        settings.DotNetSdkVersion.Returns("9.0.100");

        CapturingLogger<GlobalJsonIsLatest> logger = new();
        GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

        await check.CheckAsync(solutionFileName: "/", cancellationToken: this.CancellationToken());

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

            CapturingLogger<GlobalJsonIsLatest> logger = new();
            GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

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
    public async Task WhenGlobalJsonVersionMatchesNoErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            const string sdkVersion = "9.0.100";
            await this.WriteGlobalJsonAsync(tempDir: tempDir, version: sdkVersion, rollForward: "latestPatch");

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns(sdkVersion);

            CapturingLogger<GlobalJsonIsLatest> logger = new();
            GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

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
    public async Task WhenGlobalJsonVersionMismatchErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, version: "9.0.100", rollForward: "latestPatch");

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.200");

            CapturingLogger<GlobalJsonIsLatest> logger = new();
            GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

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
    public async Task WhenGlobalJsonHasNoRollForwardErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            await this.WriteGlobalJsonAsync(tempDir: tempDir, version: "9.0.100", rollForward: null);

            IRepositorySettings settings = Substitute.For<IRepositorySettings>();
            settings.DotNetSdkVersion.Returns("9.0.100");

            CapturingLogger<GlobalJsonIsLatest> logger = new();
            GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

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

            CapturingLogger<GlobalJsonIsLatest> logger = new();
            GlobalJsonIsLatest check = new(repositorySettings: settings, logger: logger);

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

    private Task WriteGlobalJsonAsync(string tempDir, string version, string? rollForward)
    {
        string content = rollForward is null
            ? $"{{\"sdk\":{{\"version\":\"{version}\"}}}}"
            : $"{{\"sdk\":{{\"version\":\"{version}\",\"rollForward\":\"{rollForward}\"}}}}";

        return File.WriteAllTextAsync(
            path: Path.Combine(path1: tempDir, path2: "global.json"),
            contents: content,
            cancellationToken: this.CancellationToken()
        );
    }
}
