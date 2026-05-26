using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.BuildCheck.Runner.Services;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.ReferencedPackages;

public sealed class HasAppropriatePackagesTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // UsingNSubstituteMustIncludeAnalyzer (HasAppropriateAnalysisPackages)
    // detect: NSubstitute / mustInclude: NSubstitute.Analyzers.CSharp
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenNSubstituteAndAnalyzerBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"NSubstitute\" Version=\"5.0.0\" /><PackageReference Include=\"NSubstitute.Analyzers.CSharp\" Version=\"1.0.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingNSubstituteMustIncludeAnalyzer> logger = new();
        UsingNSubstituteMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNSubstitutePresentButAnalyzerMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"NSubstitute\" Version=\"5.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingNSubstituteMustIncludeAnalyzer> logger = new();
        UsingNSubstituteMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNSubstituteNotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingNSubstituteMustIncludeAnalyzer> logger = new();
        UsingNSubstituteMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNSubstituteAndAnalyzerPresentButAnalyzerMissingPrivateAssetsErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"NSubstitute\" Version=\"5.0.0\" /><PackageReference Include=\"NSubstitute.Analyzers.CSharp\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingNSubstituteMustIncludeAnalyzer> logger = new();
        UsingNSubstituteMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // UsingXUnitMustIncludeVisualStudioTestPlatform (HasAppropriateNonAnalysisPackages)
    // detect: xunit / mustInclude: Microsoft.NET.Test.Sdk
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitAndTestSdkBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /><PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitMustIncludeVisualStudioTestPlatform> logger = new();
        UsingXUnitMustIncludeVisualStudioTestPlatform check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitPresentButTestSdkMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitMustIncludeVisualStudioTestPlatform> logger = new();
        UsingXUnitMustIncludeVisualStudioTestPlatform check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitNotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitMustIncludeVisualStudioTestPlatform> logger = new();
        UsingXUnitMustIncludeVisualStudioTestPlatform check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // HasConsistentNuGetPackages
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenHasConsistentNuGetPackagesAllPackagesHaveConsistentVersionsNoErrorIsLoggedAsync()
    {
        XmlDocument doc1 = new();
        doc1.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project1 = new(Name: "Test1.csproj", Folder: "/test", CsProjXml: doc1);

        XmlDocument doc2 = new();
        doc2.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project2 = new(Name: "Test2.csproj", Folder: "/test", CsProjXml: doc2);

        CapturingLogger<HasConsistentNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        HasConsistentNuGetPackages check = new(checkConfiguration: configuration, logger: logger);

        await check.CheckAsync(project: project1, cancellationToken: this.CancellationToken());
        await check.CheckAsync(project: project2, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenHasConsistentNuGetPackagesVersionMismatchAndNotAllowedErrorIsLoggedAsync()
    {
        XmlDocument doc1 = new();
        doc1.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project1 = new(Name: "Test1.csproj", Folder: "/test", CsProjXml: doc1);

        XmlDocument doc2 = new();
        doc2.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project2 = new(Name: "Test2.csproj", Folder: "/test", CsProjXml: doc2);

        CapturingLogger<HasConsistentNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        HasConsistentNuGetPackages check = new(checkConfiguration: configuration, logger: logger);

        await check.CheckAsync(project: project1, cancellationToken: this.CancellationToken());
        await check.CheckAsync(project: project2, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenHasConsistentNuGetPackagesVersionMismatchAndAllowedNoErrorIsLoggedAsync()
    {
        XmlDocument doc1 = new();
        doc1.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project1 = new(Name: "Test1.csproj", Folder: "/test", CsProjXml: doc1);

        XmlDocument doc2 = new();
        doc2.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project2 = new(Name: "Test2.csproj", Folder: "/test", CsProjXml: doc2);

        CapturingLogger<HasConsistentNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: true);
        HasConsistentNuGetPackages check = new(checkConfiguration: configuration, logger: logger);

        await check.CheckAsync(project: project1, cancellationToken: this.CancellationToken());
        await check.CheckAsync(project: project2, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseAbstractionsLoggingPackage (ShouldUseAlternatePackage)
    // match: Microsoft.Extensions.Logging / use: Microsoft.Extensions.Logging.Abstractions
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseAbstractionsLoggingPackageNoMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Logging.Abstractions\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsLoggingPackage> logger = new();
        ShouldUseAbstractionsLoggingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsLoggingPackageMatchPackageInLibraryErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Logging\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsLoggingPackage> logger = new();
        ShouldUseAbstractionsLoggingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseAbstractionsLoggingPackageMatchPackageInExeNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Extensions.Logging\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseAbstractionsLoggingPackage> logger = new();
        ShouldUseAbstractionsLoggingPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
