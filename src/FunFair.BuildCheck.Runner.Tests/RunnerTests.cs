using System;
using System.Collections.Generic;
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
        IFrameworkSettings frameworkSettings = GetSubstitute<IFrameworkSettings>();
        frameworkSettings.ProjectImport.Returns(string.Empty);
        frameworkSettings.DotNetAllowPreReleaseSdk.Returns(string.Empty);
        IProjectClassifier projectClassifier = GetSubstitute<IProjectClassifier>();
        ICheckConfiguration checkConfiguration = new CheckConfiguration(
            PreReleaseBuild: false,
            AllowPackageVersionMismatch: false
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
        IFrameworkSettings frameworkSettings = GetSubstitute<IFrameworkSettings>();
        frameworkSettings.ProjectImport.Returns(string.Empty);
        frameworkSettings.DotNetAllowPreReleaseSdk.Returns(string.Empty);
        IProjectClassifier projectClassifier = GetSubstitute<IProjectClassifier>();
        ICheckConfiguration checkConfiguration = new CheckConfiguration(
            PreReleaseBuild: false,
            AllowPackageVersionMismatch: false
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

    [Fact]
    public async Task CheckAsyncWithLegacySolutionWithProjectReferenceReturnsNoErrorsAsync()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnPath = Path.Combine(tempDir, "test.sln");
            await File.WriteAllTextAsync(
                slnPath,
                "Microsoft Visual Studio Solution File, Format Version 12.00\r\nProject(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"Proj\", \"Proj.csproj\", \"{12345678-1234-1234-1234-123456789ABC}\"\r\nEndProject",
                this.CancellationToken()
            );
            await File.WriteAllTextAsync(
                Path.Combine(tempDir, "Proj.csproj"),
                "<Project></Project>",
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
    public async Task CheckAsyncWithLegacySolutionHavingNoBasePathReturnsZeroAsync()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        string originalDir = Environment.CurrentDirectory;

        try
        {
            const string slnFileName = "test.sln";
            await File.WriteAllTextAsync(Path.Combine(tempDir, slnFileName), string.Empty, this.CancellationToken());

            Environment.CurrentDirectory = tempDir;

            int result = await this.RunCheckAsync(slnFileName);

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Environment.CurrentDirectory = originalDir;
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithProjectHavingEmptyFileNameLogsErrorAsync()
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
            ICheckConfiguration checkConfiguration = new CheckConfiguration(
                PreReleaseBuild: false,
                AllowPackageVersionMismatch: false
            );
            CapturingLogger logger = new();

            int result = await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: false,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: checkConfiguration,
                buildServiceProvider: StripChecksAndBuildWithEmptyPathProject,
                logger: logger,
                cancellationToken: this.CancellationToken()
            );

            Assert.Equal(expected: 1, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task RepositorySettingsDelegatesToFrameworkSettingsAsync()
    {
        IFrameworkSettings frameworkSettings = GetSubstitute<IFrameworkSettings>();
        frameworkSettings.IsNullableGloballyEnforced.Returns(true);
        frameworkSettings.ProjectImport.Returns("TestProjectImport");
        frameworkSettings.DotnetPackable.Returns("true");
        frameworkSettings.DotnetPublishable.Returns("false");
        frameworkSettings.DotnetTargetFramework.Returns("net10.0");
        frameworkSettings.DotNetSdkVersion.Returns("10.0.300");
        frameworkSettings.DotNetAllowPreReleaseSdk.Returns("false");
        frameworkSettings.XmlDocumentationRequired.Returns(true);

        IProjectClassifier projectClassifier = GetSubstitute<IProjectClassifier>();

        IRepositorySettings settings = await this.CaptureRepositorySettingsAsync(
            frameworkSettings: frameworkSettings,
            projectClassifier: projectClassifier
        );

        Assert.True(
            settings.IsNullableGloballyEnforced,
            userMessage: "IsNullableGloballyEnforced should delegate to IFrameworkSettings"
        );
        Assert.Equal(expected: "TestProjectImport", actual: settings.ProjectImport);
        Assert.Equal(expected: "true", actual: settings.DotnetPackable);
        Assert.Equal(expected: "false", actual: settings.DotnetPublishable);
        Assert.Equal(expected: "net10.0", actual: settings.DotnetTargetFramework);
        Assert.Equal(expected: "10.0.300", actual: settings.DotNetSdkVersion);
        Assert.Equal(expected: "false", actual: settings.DotNetAllowPreReleaseSdk);
        Assert.True(
            settings.XmlDocumentationRequired,
            userMessage: "XmlDocumentationRequired should delegate to IFrameworkSettings"
        );
    }

    private async Task<IRepositorySettings> CaptureRepositorySettingsAsync(
        IFrameworkSettings frameworkSettings,
        IProjectClassifier projectClassifier
    )
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            string slnxPath = Path.Combine(tempDir, "test.slnx");
            await File.WriteAllTextAsync(slnxPath, "<Solution></Solution>", this.CancellationToken());

            IRepositorySettings? capturedSettings = null;
            CapturingLogger logger = new();

            await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: false,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: new CheckConfiguration(PreReleaseBuild: false, AllowPackageVersionMismatch: false),
                buildServiceProvider: CaptureSettingsAndBuild,
                logger: logger,
                cancellationToken: this.CancellationToken()
            );

            return capturedSettings ?? throw new InvalidOperationException("Repository settings were not captured");

            IServiceProvider CaptureSettingsAndBuild(IServiceCollection services)
            {
                IServiceProvider sp = StripChecksAndBuild(services);
                capturedSettings = sp.GetRequiredService<IRepositorySettings>();

                return sp;
            }
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithCodeAnalysisSolutionCoversSetupBranchAsync()
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
            projectClassifier.IsCodeAnalysisSolution(Arg.Any<IReadOnlyList<SolutionProject>>()).Returns(true);
            ICheckConfiguration checkConfiguration = new CheckConfiguration(
                PreReleaseBuild: false,
                AllowPackageVersionMismatch: false
            );
            CapturingLogger logger = new();

            int result = await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: false,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: checkConfiguration,
                buildServiceProvider: StripChecksAndBuild,
                logger: logger,
                cancellationToken: this.CancellationToken()
            );

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithEnumSourceGeneratorRequiredCoversSetupBranchAsync()
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
            projectClassifier
                .MustHaveEnumSourceGeneratorAnalyzerPackage(Arg.Any<IReadOnlyList<SolutionProject>>())
                .Returns(true);
            ICheckConfiguration checkConfiguration = new CheckConfiguration(
                PreReleaseBuild: false,
                AllowPackageVersionMismatch: false
            );
            CapturingLogger logger = new();

            int result = await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: false,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: checkConfiguration,
                buildServiceProvider: StripChecksAndBuild,
                logger: logger,
                cancellationToken: this.CancellationToken()
            );

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public async Task CheckAsyncWithUnitTestBaseCoversSetupBranchAsync()
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
            projectClassifier.IsUnitTestBase(Arg.Any<IReadOnlyList<SolutionProject>>()).Returns(true);
            ICheckConfiguration checkConfiguration = new CheckConfiguration(
                PreReleaseBuild: false,
                AllowPackageVersionMismatch: false
            );
            CapturingLogger logger = new();

            int result = await CheckRunner.CheckAsync(
                solutionFileName: slnxPath,
                warningsAsErrors: false,
                frameworkSettings: frameworkSettings,
                projectClassifier: projectClassifier,
                checkConfiguration: checkConfiguration,
                buildServiceProvider: StripChecksAndBuild,
                logger: logger,
                cancellationToken: this.CancellationToken()
            );

            Assert.Equal(expected: 0, actual: result);
        }
        finally
        {
            Directory.Delete(tempDir, recursive: true);
        }
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

    private static IServiceProvider StripChecksAndBuildWithEmptyPathProject(IServiceCollection services)
    {
        ServiceDescriptor? existingProjects = services.FirstOrDefault(s =>
            s.ServiceType == typeof(IReadOnlyList<SolutionProject>)
        );

        if (existingProjects is not null)
        {
            services.Remove(existingProjects);
        }

        services.AddSingleton<IReadOnlyList<SolutionProject>>([
            new(FileName: string.Empty, DisplayName: "InvalidProject"),
        ]);

        return StripChecksAndBuild(services);
    }
}
