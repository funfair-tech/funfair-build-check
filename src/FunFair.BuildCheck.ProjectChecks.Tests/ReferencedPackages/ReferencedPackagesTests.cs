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

public sealed class ReferencedPackagesTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // DoesNotUseDotNetCliToolReference
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDoesNotUseDotNetCliToolReferenceProjectHasNoToolReferencesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup></ItemGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotUseDotNetCliToolReference> logger = new();
        DoesNotUseDotNetCliToolReference check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDoesNotUseDotNetCliToolReferenceProjectHasToolReferenceErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><DotNetCliToolReference Include=\"SomeTool\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotUseDotNetCliToolReference> logger = new();
        DoesNotUseDotNetCliToolReference check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ReferencesNugetPackageOnlyOnce
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenReferencesNugetPackageOnlyOnceProjectHasUniquePackagesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /><PackageReference Include=\"Bar\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ReferencesNugetPackageOnlyOnce> logger = new();
        ReferencesNugetPackageOnlyOnce check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenReferencesNugetPackageOnlyOnceProjectHasDuplicatePackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ReferencesNugetPackageOnlyOnce> logger = new();
        ReferencesNugetPackageOnlyOnce check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenReferencesNugetPackageOnlyOnceDuplicatePrivateAssetsAllSkippedNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" PrivateAssets=\"All\" /><PackageReference Include=\"Foo\" Version=\"1.0.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ReferencesNugetPackageOnlyOnce> logger = new();
        ReferencesNugetPackageOnlyOnce check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenReferencesNugetPackageOnlyOncePackageReferenceWithMissingIncludeAttributeErrorIsLoggedAsync()
    {
        // A PackageReference element without an Include attribute triggers a bad reference warning
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ReferencesNugetPackageOnlyOnce> logger = new();
        ReferencesNugetPackageOnlyOnce check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotDisableUnexpectedWarnings
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsProjectHasNoNoWarnNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsProjectHasAllowedNoWarnNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><NoWarn>1591</NoWarn></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsProjectHasDisallowedNoWarnErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><NoWarn>CS0649</NoWarn></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsTestProjectHasAllowedNoWarnNoErrorIsLoggedAsync()
    {
        // Test projects have an empty allowed warnings list — 1591 is allowed for non-test projects only
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><NoWarn>1591</NoWarn></PropertyGroup><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsConfigurationGroupHasDisallowedNoWarnErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup Condition=\"'$(Configuration)'=='Debug'\"><NoWarn>CS0649</NoWarn></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsConfigurationGroupHasGlobalNoWarnReferenceNoErrorIsLoggedAsync()
    {
        // $(NoWarn) references the global NoWarn — this is allowed
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup Condition=\"'$(Configuration)'=='Debug'\"><NoWarn>$(NoWarn)</NoWarn></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDisableUnexpectedWarningsConfigurationGroupHasEmptyNoWarnNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup Condition=\"'$(Configuration)'=='Debug'\"><NoWarn></NoWarn></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDisableUnexpectedWarnings> logger = new();
        MustNotDisableUnexpectedWarnings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotHaveFxCopAnalyzerPackage
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotHaveFxCopAnalyzerPackageProjectHasNoSuchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotHaveFxCopAnalyzerPackage> logger = new();
        MustNotHaveFxCopAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotHaveFxCopAnalyzerPackageProjectHasFxCopAnalyzerPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.CodeAnalysis.FxCopAnalyzers\" Version=\"3.3.2\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotHaveFxCopAnalyzerPackage> logger = new();
        MustNotHaveFxCopAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotReferenceMicrosoftVisualStudioThreading
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotReferenceMicrosoftVisualStudioThreadingProjectHasNoSuchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceMicrosoftVisualStudioThreading> logger = new();
        MustNotReferenceMicrosoftVisualStudioThreading check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotReferenceMicrosoftVisualStudioThreadingProjectHasPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.VisualStudio.Threading\" Version=\"17.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceMicrosoftVisualStudioThreading> logger = new();
        MustNotReferenceMicrosoftVisualStudioThreading check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotReferenceObsoleteAspNetPackages
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotReferenceObsoleteAspNetPackagesProjectHasNoObsoletePackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceObsoleteAspNetPackages> logger = new();
        MustNotReferenceObsoleteAspNetPackages check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotReferenceObsoleteAspNetPackagesProjectHasObsoletePackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.AspNetCore\" Version=\"2.2.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceObsoleteAspNetPackages> logger = new();
        MustNotReferenceObsoleteAspNetPackages check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotReferenceTeamCityTestAdapter
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotReferenceTeamCityTestAdapterProjectHasNoSuchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceTeamCityTestAdapter> logger = new();
        MustNotReferenceTeamCityTestAdapter check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotReferenceTeamCityTestAdapterProjectHasPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"TeamCity.VSTest.TestAdapter\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotReferenceTeamCityTestAdapter> logger = new();
        MustNotReferenceTeamCityTestAdapter check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotUseReferenceCoverlet
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotUseReferenceCoverletProjectHasNoSuchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseReferenceCoverlet> logger = new();
        MustNotUseReferenceCoverlet check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotUseReferenceCoverletProjectHasCoverletCollectorPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"coverlet.collector\" Version=\"6.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseReferenceCoverlet> logger = new();
        MustNotUseReferenceCoverlet check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldNotReferenceAllMetaPackagesInPackableProjects
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldNotReferenceAllMetaPackagesProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"Some.All\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger = new();
        ShouldNotReferenceAllMetaPackagesInPackableProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldNotReferenceAllMetaPackagesPackableProjectHasMetaPackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Some.All\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger = new();
        ShouldNotReferenceAllMetaPackagesInPackableProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldNotReferenceAllMetaPackagesPackableProjectHasNonMetaPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger = new();
        ShouldNotReferenceAllMetaPackagesInPackableProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseNSubstituteRatherThanMoqPackage
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseNSubstituteRatherThanMoqPackageNoMoqReferenceNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"NSubstitute\" Version=\"5.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseNSubstituteRatherThanMoqPackage> logger = new();
        ShouldUseNSubstituteRatherThanMoqPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseNSubstituteRatherThanMoqPackageMoqReferenceErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"Moq\" Version=\"4.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseNSubstituteRatherThanMoqPackage> logger = new();
        ShouldUseNSubstituteRatherThanMoqPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldUseCredfetoVersionInfoProjectHasNoMatchPackageNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomePackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo> logger = new();
        ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldUseCredfetoVersionInfoProjectHasObsoletePackageErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"ThisAssembly.AssemblyInfo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo> logger = new();
        ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
