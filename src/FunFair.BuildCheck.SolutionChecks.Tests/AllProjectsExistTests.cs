using System;
using System.IO;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.SolutionChecks.Tests;

public sealed class AllProjectsExistTests : TestBase
{
    [Fact]
    public async Task WhenAllProjectsExistNoErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            string projectFile = Path.Combine(path1: tempDir, path2: "Sample.csproj");
            await File.WriteAllTextAsync(
                path: projectFile,
                contents: "<Project />",
                cancellationToken: this.CancellationToken()
            );

            SolutionProject[] projects = [new(fileName: projectFile, displayName: "Sample")];

            CapturingLogger<AllProjectsExist> logger = new();
            AllProjectsExist check = new(projects: projects, logger: logger);

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
    public async Task WhenProjectDoesNotExistErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            string missingProject = Path.Combine(path1: tempDir, path2: "Missing.csproj");

            SolutionProject[] projects = [new(fileName: missingProject, displayName: "Missing")];

            CapturingLogger<AllProjectsExist> logger = new();
            AllProjectsExist check = new(projects: projects, logger: logger);

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
    public async Task WhenNoProjectsExistNoErrorIsLoggedAsync()
    {
        string tempDir = CreateTempDirectory();

        try
        {
            CapturingLogger<AllProjectsExist> logger = new();
            AllProjectsExist check = new(projects: [], logger: logger);

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

    private static string CreateTempDirectory()
    {
        string path = Path.Combine(path1: Path.GetTempPath(), path2: Path.GetRandomFileName());
        Directory.CreateDirectory(path);

        return path;
    }
}
