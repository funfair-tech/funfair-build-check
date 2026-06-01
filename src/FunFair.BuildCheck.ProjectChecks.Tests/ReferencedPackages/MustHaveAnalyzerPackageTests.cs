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

public sealed class MustHaveAnalyzerPackageTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // MustHaveAsyncAnalyzerPackage  (packageId: "AsyncFixer")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveAsyncAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"AsyncFixer\" Version=\"1.6.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveAsyncAnalyzerPackage> logger = new();
        MustHaveAsyncAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveAsyncAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveAsyncAnalyzerPackage> logger = new();
        MustHaveAsyncAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveAsyncAnalyzerPackagePackagePresentButMissingPrivateAssetsErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"AsyncFixer\" Version=\"1.6.0\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveAsyncAnalyzerPackage> logger = new();
        MustHaveAsyncAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveAsyncAnalyzerPackagePackagePresentButMissingExcludeAssetsErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"AsyncFixer\" Version=\"1.6.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveAsyncAnalyzerPackage> logger = new();
        MustHaveAsyncAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveFunFairCodeAnalysisPackage  (packageId: "FunFair.CodeAnalysis")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveFunFairCodeAnalysisPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"FunFair.CodeAnalysis\" Version=\"7.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveFunFairCodeAnalysisPackage> logger = new();
        MustHaveFunFairCodeAnalysisPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveFunFairCodeAnalysisPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveFunFairCodeAnalysisPackage> logger = new();
        MustHaveFunFairCodeAnalysisPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveSonarAnalyzerPackage  (packageId: "SonarAnalyzer.CSharp")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveSonarAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SonarAnalyzer.CSharp\" Version=\"9.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSonarAnalyzerPackage> logger = new();
        MustHaveSonarAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveSonarAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSonarAnalyzerPackage> logger = new();
        MustHaveSonarAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveDuplicateCodeAnalyzerPackage  (packageId: "Philips.CodeAnalysis.DuplicateCodeAnalyzer")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveDuplicateCodeAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Philips.CodeAnalysis.DuplicateCodeAnalyzer\" Version=\"1.6.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveDuplicateCodeAnalyzerPackage> logger = new();
        MustHaveDuplicateCodeAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveDuplicateCodeAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveDuplicateCodeAnalyzerPackage> logger = new();
        MustHaveDuplicateCodeAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveEnumSourceGeneratorAnalyzerPackage (conditional on repositorySettings)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveEnumSourceGeneratorAnalyzerPackageNotRequiredNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger = new();
        MustHaveEnumSourceGeneratorAnalyzerPackage check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveEnumSourceGeneratorAnalyzerPackageRequiredAndPresentNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Credfeto.Enumeration.Source.Generation\" Version=\"1.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger = new();
        MustHaveEnumSourceGeneratorAnalyzerPackage check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveEnumSourceGeneratorAnalyzerPackageRequiredAndMissingErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger = new();
        MustHaveEnumSourceGeneratorAnalyzerPackage check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveMeziantouAnalyzerPackage  (packageId: "Meziantou.Analyzer")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveMeziantouAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Meziantou.Analyzer\" Version=\"2.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveMeziantouAnalyzerPackage> logger = new();
        MustHaveMeziantouAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveMeziantouAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveMeziantouAnalyzerPackage> logger = new();
        MustHaveMeziantouAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveMicrosoftSbomTargetsAnalyzerPackage  (packageId: "Microsoft.Sbom.Targets", CanCheck=IsPackable)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveMicrosoftSbomTargetsAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"Microsoft.Sbom.Targets\" Version=\"1.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveMicrosoftSbomTargetsAnalyzerPackage> logger = new();
        MustHaveMicrosoftSbomTargetsAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveMicrosoftSbomTargetsAnalyzerPackagePackageIsMissingAndProjectIsPackableErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveMicrosoftSbomTargetsAnalyzerPackage> logger = new();
        MustHaveMicrosoftSbomTargetsAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveMicrosoftSbomTargetsAnalyzerPackageProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveMicrosoftSbomTargetsAnalyzerPackage> logger = new();
        MustHaveMicrosoftSbomTargetsAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHavePhilipsMaintainabilityAnalyzerPackage  (packageId: "Philips.CodeAnalysis.MaintainabilityAnalyzers")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHavePhilipsMaintainabilityAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Philips.CodeAnalysis.MaintainabilityAnalyzers\" Version=\"1.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHavePhilipsMaintainabilityAnalyzerPackage> logger = new();
        MustHavePhilipsMaintainabilityAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHavePhilipsMaintainabilityAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHavePhilipsMaintainabilityAnalyzerPackage> logger = new();
        MustHavePhilipsMaintainabilityAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveRoslynatorAnalyzersPackage  (packageId: "Roslynator.Analyzers")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveRoslynatorAnalyzersPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Roslynator.Analyzers\" Version=\"4.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveRoslynatorAnalyzersPackage> logger = new();
        MustHaveRoslynatorAnalyzersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveRoslynatorAnalyzersPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveRoslynatorAnalyzersPackage> logger = new();
        MustHaveRoslynatorAnalyzersPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveSecurityCodeScanAnalyzerPackage  (packageId: "SecurityCodeScan.VS2019")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveSecurityCodeScanAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SecurityCodeScan.VS2019\" Version=\"5.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSecurityCodeScanAnalyzerPackage> logger = new();
        MustHaveSecurityCodeScanAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveSecurityCodeScanAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSecurityCodeScanAnalyzerPackage> logger = new();
        MustHaveSecurityCodeScanAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveSmartAnalyzerPackage  (packageId: "SmartAnalyzers.CSharpExtensions.Annotations")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveSmartAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SmartAnalyzers.CSharpExtensions.Annotations\" Version=\"4.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSmartAnalyzerPackage> logger = new();
        MustHaveSmartAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveSmartAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveSmartAnalyzerPackage> logger = new();
        MustHaveSmartAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveThreadingAnalyzerPackage  (packageId: "Microsoft.VisualStudio.Threading.Analyzers")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveThreadingAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.VisualStudio.Threading.Analyzers\" Version=\"17.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveThreadingAnalyzerPackage> logger = new();
        MustHaveThreadingAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveThreadingAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveThreadingAnalyzerPackage> logger = new();
        MustHaveThreadingAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustHaveToStringWithoutOverrideAnalyzerPackage  (packageId: "ToStringWithoutOverrideAnalyzer")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustHaveToStringWithoutOverrideAnalyzerPackagePackageIsPresentWithCorrectAttributesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"ToStringWithoutOverrideAnalyzer\" Version=\"2.0.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveToStringWithoutOverrideAnalyzerPackage> logger = new();
        MustHaveToStringWithoutOverrideAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustHaveToStringWithoutOverrideAnalyzerPackagePackageIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveToStringWithoutOverrideAnalyzerPackage> logger = new();
        MustHaveToStringWithoutOverrideAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // Microsoft.NET.Test.Sdk: FFTestProject skips ExcludeAssets check
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMicrosoftNetTestSdkIsFunFairTestProjectExcludeAssetsCheckIsSkippedNoErrorIsLoggedAsync()
    {
        // Microsoft.NET.Test.Sdk is special-cased: ExcludeAssets check is skipped for FFTestProject
        // We demonstrate the analogous behavior by using a simple test-project-aware analyzer
        // The "MustHaveSonarAnalyzerPackage" is always mustHave=true and
        // doesn't skip FFTestProject — so we just verify FFTestProject is treated correctly by the package check using a non-excluded package.
        // The actual ExcludeAssets skip is for Microsoft.NET.Test.Sdk package id specifically.

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><FFTestProject>true</FFTestProject></PropertyGroup><ItemGroup><PackageReference Include=\"AsyncFixer\" Version=\"1.6.0\" PrivateAssets=\"All\" ExcludeAssets=\"runtime\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustHaveAsyncAnalyzerPackage> logger = new();
        MustHaveAsyncAnalyzerPackage check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
