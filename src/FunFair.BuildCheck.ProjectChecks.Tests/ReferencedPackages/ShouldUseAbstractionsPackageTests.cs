using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.ReferencedPackages;

public sealed class ShouldUseAbstractionsPackageTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsCachingPackage
    // matchPackage: Microsoft.Extensions.Caching.Memory
    // usePackage: Microsoft.Extensions.Caching.Abstractions
    // ShouldExclude returns true for test projects
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsCachingPackageProjectDoesNotReferenceMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsCachingPackage> logger = new();
        ShouldUseAbstractionsCachingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsCachingPackageLibraryProjectReferencesMatchPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Caching.Memory\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsCachingPackage> logger = new();
        ShouldUseAbstractionsCachingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsCachingPackageExeProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Caching.Memory\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsCachingPackage> logger = new();
        ShouldUseAbstractionsCachingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsCachingPackageLambdaProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><AWSProjectType>Lambda</AWSProjectType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Caching.Memory\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsCachingPackage> logger = new();
        ShouldUseAbstractionsCachingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsCachingPackageTestProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /><PackageReference Include=\"Microsoft.Extensions.Caching.Memory\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsCachingPackage> logger = new();
        ShouldUseAbstractionsCachingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsConfigurationPackage
    // matchPackage: Microsoft.Extensions.Configuration
    // usePackage: Microsoft.Extensions.Configuration.Abstractions
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsConfigurationPackageProjectDoesNotReferenceMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsConfigurationPackage> logger = new();
        ShouldUseAbstractionsConfigurationPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsConfigurationPackageLibraryProjectReferencesMatchPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Configuration\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsConfigurationPackage> logger = new();
        ShouldUseAbstractionsConfigurationPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsConfigurationPackageExeProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Configuration\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsConfigurationPackage> logger = new();
        ShouldUseAbstractionsConfigurationPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsConfigurationPackageLambdaProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><AWSProjectType>Lambda</AWSProjectType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Configuration\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsConfigurationPackage> logger = new();
        ShouldUseAbstractionsConfigurationPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsDependencyInjectionPackage
    // matchPackage: Microsoft.Extensions.DependencyInjection
    // usePackage: Microsoft.Extensions.DependencyInjection.Abstractions
    // CanCheck: returns false if repositorySettings.IsUnitTestBase && IsTestProject && !Name.EndsWith(".Tests")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsDependencyInjectionPackageProjectDoesNotReferenceMatchPackageNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsDependencyInjectionPackage> logger = new();
        ShouldUseAbstractionsDependencyInjectionPackage check = new(
            repositorySettings: repositorySettings,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsDependencyInjectionPackageLibraryProjectReferencesMatchPackageErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.DependencyInjection\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsDependencyInjectionPackage> logger = new();
        ShouldUseAbstractionsDependencyInjectionPackage check = new(
            repositorySettings: repositorySettings,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsDependencyInjectionPackageExeProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.DependencyInjection\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsDependencyInjectionPackage> logger = new();
        ShouldUseAbstractionsDependencyInjectionPackage check = new(
            repositorySettings: repositorySettings,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsDependencyInjectionPackageIsUnitTestBaseAndTestProjectNotEndingInTestsCanCheckReturnsFalseNoErrorIsLoggedAsync()
    {
        // CanCheck returns false when IsUnitTestBase=true AND IsTestProject AND Name does not end with ".Tests"
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.IsUnitTestBase.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /><PackageReference Include=\"Microsoft.Extensions.DependencyInjection\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        // Name does NOT end with ".Tests" (display name, no .csproj suffix)
        ProjectContext project = new(Name: "FunFair.Test.Common", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsDependencyInjectionPackage> logger = new();
        ShouldUseAbstractionsDependencyInjectionPackage check = new(
            repositorySettings: repositorySettings,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsDependencyInjectionPackageIsUnitTestBaseAndTestProjectEndingInTestsCanCheckReturnsTrueErrorIsLoggedAsync()
    {
        // CanCheck returns true when IsUnitTestBase=true AND IsTestProject AND Name ends with ".Tests"
        // Name is the project display name (file name without extension), so no .csproj suffix
        IRepositorySettings repositorySettings = GetSubstitute<IRepositorySettings>();
        repositorySettings.IsUnitTestBase.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /><PackageReference Include=\"Microsoft.Extensions.DependencyInjection\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        // Name ends with ".Tests" (display name, no .csproj suffix)
        ProjectContext project = new(Name: "SomeProject.Tests", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsDependencyInjectionPackage> logger = new();
        ShouldUseAbstractionsDependencyInjectionPackage check = new(
            repositorySettings: repositorySettings,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsExtensionHostingPackage
    // matchPackage: Microsoft.Extensions.Hosting
    // usePackage: Microsoft.Extensions.Hosting.Abstractions
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsExtensionHostingPackageProjectDoesNotReferenceMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsExtensionHostingPackage> logger = new();
        ShouldUseAbstractionsExtensionHostingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsExtensionHostingPackageLibraryProjectReferencesMatchPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Hosting\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsExtensionHostingPackage> logger = new();
        ShouldUseAbstractionsExtensionHostingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsExtensionHostingPackageExeProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Hosting\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsExtensionHostingPackage> logger = new();
        ShouldUseAbstractionsExtensionHostingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsExtensionHostingPackageLambdaProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><AWSProjectType>Lambda</AWSProjectType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Hosting\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsExtensionHostingPackage> logger = new();
        ShouldUseAbstractionsExtensionHostingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsFileProvidersPackage
    // matchPackage: Microsoft.Extensions.FileProviders
    // usePackage: Microsoft.Extensions.FileProviders.Abstractions
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsFileProvidersPackageProjectDoesNotReferenceMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsFileProvidersPackage> logger = new();
        ShouldUseAbstractionsFileProvidersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsFileProvidersPackageLibraryProjectReferencesMatchPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.FileProviders\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsFileProvidersPackage> logger = new();
        ShouldUseAbstractionsFileProvidersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsFileProvidersPackageExeProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.FileProviders\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsFileProvidersPackage> logger = new();
        ShouldUseAbstractionsFileProvidersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsFileProvidersPackageLambdaProjectReferencesMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><AWSProjectType>Lambda</AWSProjectType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.FileProviders\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsFileProvidersPackage> logger = new();
        ShouldUseAbstractionsFileProvidersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
