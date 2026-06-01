using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class OnlyExesShouldBePublishablePolicyTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = null (empty) → check is skipped
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsNullCheckIsSkippedNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns((string?)null);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = NONE → nothing should be publishable, error if IsPublishable=true
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsNoneAndProjectHasIsPublishableTrueErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("NONE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsNoneAndProjectHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("NONE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = ALL → all non-test projects should be publishable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsAllAndNonTestProjectHasIsPublishableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsAllAndNonTestProjectMissingIsPublishableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsAllAndTestProjectHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = EXE_TOOL → Exe and DotNet tools should be publishable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsExeToolAndExeProjectHasIsPublishableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsExeToolAndDotNetToolHasIsPublishableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>True</PackAsTool><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyTool.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsExeToolAndLibraryProjectHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyLib.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsExeToolAndExeProjectMissingIsPublishableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = EXE → only Exe projects should be publishable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsExeAndExeProjectHasIsPublishableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsExeAndLibraryProjectHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyLib.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsExeAndExeProjectMissingIsPublishableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("EXE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPublishable = comma-separated project list
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsProjectListAndProjectIsInListAndIsPublishableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("MyApp.csproj, OtherApp.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsProjectListAndProjectIsNotInListHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("MyApp.csproj, OtherApp.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "AnotherApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPublishableIsProjectListAndProjectIsInListButMissingIsPublishableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("MyApp.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // IsUnitTestBase behaviour
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPublishableIsAllAndIsUnitTestBaseAndTestProjectEndingInTestsHasIsPublishableFalseNoErrorIsLoggedAsync()
    {
        // Display name (without .csproj extension) ends with ".Tests" so is treated as a test project.
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPublishable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        // Display name ends with ".Tests"
        ProjectContext project = new(Name: "SomeProject.Tests", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OnlyExesShouldBePublishablePolicy> logger = new();
        OnlyExesShouldBePublishablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
