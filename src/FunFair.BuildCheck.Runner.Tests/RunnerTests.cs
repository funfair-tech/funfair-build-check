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

namespace FunFair.BuildCheck.Runner.Tests;

public sealed class RunnerTests : TestBase
{
    [Fact]
    public Task CheckAsyncThrowsArgumentOutOfRangeExceptionForNonSolutionFileAsync()
    {
        IFrameworkSettings frameworkSettings = Substitute.For<IFrameworkSettings>();
        frameworkSettings.ProjectImport.Returns(string.Empty);
        frameworkSettings.DotNetAllowPreReleaseSdk.Returns(string.Empty);
        IProjectClassifier projectClassifier = Substitute.For<IProjectClassifier>();
        ICheckConfiguration checkConfiguration = new CheckConfiguration(
            preReleaseBuild: false,
            allowPackageVersionMismatch: false
        );
        CapturingLogger logger = new();

        return Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            CheckRunner
                .CheckAsync(
                    solutionFileName: "test.txt",
                    warningsAsErrors: false,
                    frameworkSettings: frameworkSettings,
                    projectClassifier: projectClassifier,
                    checkConfiguration: checkConfiguration,
                    buildServiceProvider: StripChecksAndBuild,
                    logger: logger,
                    cancellationToken: this.CancellationToken()
                )
                .AsTask()
        );
    }

    [Fact]
    public async Task CheckAsyncWithEmptyLegacySolutionFileReturnsNoErrorsAsync()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnPath = Path.Combine(tempDir, "test.sln");
            await File.WriteAllTextAsync(
                slnPath,
                "Microsoft Visual Studio Solution File, Format Version 12.00\n",
                this.CancellationToken()
            );

            int result = await this.RunCheckAsync(slnPath);

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithEmptySlnxSolutionFileReturnsNoErrorsAsync()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnxPath = Path.Combine(tempDir, "test.slnx");
            await File.WriteAllTextAsync(slnxPath, "<Solution></Solution>", this.CancellationToken());

            int result = await this.RunCheckAsync(slnxPath);

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithSlnxSolutionHavingProjectCoversProjectCheckPathAsync()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnxPath = Path.Combine(tempDir, "test.slnx");
            await File.WriteAllTextAsync(
                slnxPath,
                "<Solution><Project Path=\"Proj.csproj\" /></Solution>",
                this.CancellationToken()
            );

            string projPath = Path.Combine(tempDir, "Proj.csproj");
            await File.WriteAllTextAsync(projPath, "<Project></Project>", this.CancellationToken());

            int result = await this.RunCheckAsync(slnxPath);

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    private async Task<int> RunCheckAsync(string solutionFileName)
    {
        IFrameworkSettings frameworkSettings = Substitute.For<IFrameworkSettings>();
        frameworkSettings.ProjectImport.Returns(string.Empty);
        frameworkSettings.DotNetAllowPreReleaseSdk.Returns(string.Empty);
        IProjectClassifier projectClassifier = Substitute.For<IProjectClassifier>();
        ICheckConfiguration checkConfiguration = new CheckConfiguration(
            preReleaseBuild: false,
            allowPackageVersionMismatch: false
        );
        CapturingLogger logger = new();

        return await CheckRunner.CheckAsync(
            solutionFileName: solutionFileName,
            warningsAsErrors: false,
            frameworkSettings: frameworkSettings,
            projectClassifier: projectClassifier,
            checkConfiguration: checkConfiguration,
            buildServiceProvider: StripChecksAndBuild,
            logger: logger,
            cancellationToken: this.CancellationToken()
        );
    }

    private static IServiceProvider StripChecksAndBuild(IServiceCollection services)
    {
        ServiceDescriptor[] toRemove =
        [
            .. services.Where(s => s.ServiceType == typeof(ISolutionCheck) || s.ServiceType == typeof(IProjectCheck)),
        ];

        foreach (ServiceDescriptor descriptor in toRemove)
        {
            services.Remove(descriptor);
        }

        return services.BuildServiceProvider();
    }
}
