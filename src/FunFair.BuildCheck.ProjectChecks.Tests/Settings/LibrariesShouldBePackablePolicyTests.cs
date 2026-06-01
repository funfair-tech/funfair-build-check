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

public sealed class LibrariesShouldBePackablePolicyTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = null (empty) → check is skipped
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsNullCheckIsSkippedNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns((string?)null);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = NONE → nothing should be packable, error if IsPackable=true
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsNoneAndProjectHasIsPackableTrueErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("NONE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsNoneAndProjectHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("NONE");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = ALL → all non-test projects should be packable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsAllAndNonTestProjectHasIsPackableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsAllAndNonTestProjectMissingIsPackableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsAllAndTestProjectHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = LIBRARIES → library non-test non-tool projects should be packable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsLibrariesAndLibraryProjectHasIsPackableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("LIBRARIES");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        // Library output type (default), not a tool, not a test project
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyLib.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsLibrariesAndLibraryProjectMissingIsPackableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("LIBRARIES");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "MyLib.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsLibrariesAndExeProjectHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("LIBRARIES");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyApp.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = LIBRARY_TOOL → libraries and dotnet tools should be packable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsLibraryToolAndDotNetToolHasIsPackableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("LIBRARY_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>True</PackAsTool><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyTool.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsLibraryToolAndDotNetToolMissingIsPackableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("LIBRARY_TOOL");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>True</PackAsTool></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyTool.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = TOOLS → only dotnet tools should be packable
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsToolsAndLibraryProjectWithPackAsToolHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        // TOOLS mode compares against "Library" output type. PackAsTool=True projects still have Library output type,
        // so isDotNetTool=true but isLibrary=true → policy returns false (not expected to be packable).
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("TOOLS");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>True</PackAsTool><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyTool.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsToolsAndLibraryProjectHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("TOOLS");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        // Regular library (no PackAsTool) should NOT be packable with TOOLS mode
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyLib.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotnetPackable = comma-separated project list
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsProjectListAndProjectIsInListAndIsPackableTrueNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("MyProject.csproj, OtherProject.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsProjectListAndProjectIsNotInListHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("MyProject.csproj, OtherProject.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "AnotherProject.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsProjectListAndProjectIsInListButMissingIsPackableErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("MyProject.csproj");
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "MyProject.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // IsUnitTestBase behaviour — test projects ending in .Tests are still treated as test projects
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotnetPackableIsAllAndIsUnitTestBaseAndTestProjectEndingInTestsHasIsPackableFalseNoErrorIsLoggedAsync()
    {
        // Display name (without .csproj extension) ends with ".Tests" so is treated as a test project.
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        // Display name ends with ".Tests"
        ProjectContext project = new(Name: "SomeProject.Tests", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotnetPackableIsAllAndIsUnitTestBaseAndTestProjectNotEndingInTestsHasIsPackableTrueNoErrorIsLoggedAsync()
    {
        // When IsUnitTestBase=true and it's a test project but NOT ending in .Tests,
        // IsTestProject() returns false (not treated as a test project for the purpose of exclusion),
        // so the ALL policy will require IsPackable=true.
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.DotnetPackable.Returns("ALL");
        repositorySettings.IsUnitTestBase.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        // Display name does NOT end with ".Tests"
        ProjectContext project = new(Name: "FunFair.Test.Common", Folder: "/test", CsProjXml: doc);

        CapturingLogger<LibrariesShouldBePackablePolicy> logger = new();
        LibrariesShouldBePackablePolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
