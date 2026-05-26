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

public sealed class CanCheckOverrideTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldBeAnExecutable
    // CanCheck = IsTestProject && ReferencesPackage("xunit.v3")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldBeAnExecutablePropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldBeAnExecutable> logger = new();
        XUnitV3ProjectsShouldBeAnExecutable check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldBeAnExecutablePropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldBeAnExecutable> logger = new();
        XUnitV3ProjectsShouldBeAnExecutable check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldBeAnExecutableCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        // Not a test project (no xunit reference) → CanCheck returns false
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldBeAnExecutable> logger = new();
        XUnitV3ProjectsShouldBeAnExecutable check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport
    // CanCheck = IsTestProject && (xunit.v3 || xunit.v3.extensibility.core)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        // Not a test project (no xunit reference) → CanCheck returns false
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner
    // CanCheck = IsTestProject && (xunit.v3 || xunit.v3.extensibility.core)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><UseMicrosoftTestingPlatformRunner>true</UseMicrosoftTestingPlatformRunner></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner> logger = new();
        XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner> logger = new();
        XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        // Not a test project → CanCheck returns false
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner> logger = new();
        XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // JsonSerializerIsReflectionEnabledByDefaultPolicy
    // CanCheck = IsPackable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenJsonSerializerIsReflectionEnabledByDefaultPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><JsonSerializerIsReflectionEnabledByDefault>false</JsonSerializerIsReflectionEnabledByDefault></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> logger = new();
        JsonSerializerIsReflectionEnabledByDefaultPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenJsonSerializerIsReflectionEnabledByDefaultPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> logger = new();
        JsonSerializerIsReflectionEnabledByDefaultPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenJsonSerializerIsReflectionEnabledByDefaultPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> logger = new();
        JsonSerializerIsReflectionEnabledByDefaultPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy
    // CanCheck = IsPackable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><EnableMicrosoftExtensionsConfigurationBinderSourceGenerator>true</EnableMicrosoftExtensionsConfigurationBinderSourceGenerator></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger = new();
        EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger = new();
        EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger = new();
        EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // EnablePackageValidationPolicy
    // CanCheck = IsPackable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenEnablePackageValidationPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><EnablePackageValidation>true</EnablePackageValidation></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnablePackageValidationPolicy> logger = new();
        EnablePackageValidationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenEnablePackageValidationPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnablePackageValidationPolicy> logger = new();
        EnablePackageValidationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenEnablePackageValidationPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<EnablePackageValidationPolicy> logger = new();
        EnablePackageValidationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // GenerateSbomPolicy
    // CanCheck = IsPackable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenGenerateSbomPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><GenerateSBOM>true</GenerateSBOM></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<GenerateSbomPolicy> logger = new();
        GenerateSbomPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenGenerateSbomPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<GenerateSbomPolicy> logger = new();
        GenerateSbomPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenGenerateSbomPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<GenerateSbomPolicy> logger = new();
        GenerateSbomPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // IncludeSymbolsPolicy
    // CanCheck = IsPackable() && !IsDotNetTool()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenIncludeSymbolsPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><IncludeSymbols>true</IncludeSymbols></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IncludeSymbolsPolicy> logger = new();
        IncludeSymbolsPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenIncludeSymbolsPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IncludeSymbolsPolicy> logger = new();
        IncludeSymbolsPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenIncludeSymbolsPolicyCanCheckIsFalseBecauseNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IncludeSymbolsPolicy> logger = new();
        IncludeSymbolsPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // OptimizationPreferencePolicy
    // CanCheck = IsPackable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenOptimizationPreferencePolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable><OptimizationPreference>speed</OptimizationPreference></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OptimizationPreferencePolicy> logger = new();
        OptimizationPreferencePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenOptimizationPreferencePolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>true</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OptimizationPreferencePolicy> logger = new();
        OptimizationPreferencePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenOptimizationPreferencePolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<OptimizationPreferencePolicy> logger = new();
        OptimizationPreferencePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TieredCompilationPolicy
    // CanCheck = IsPublishable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTieredCompilationPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable><TieredCompilation>true</TieredCompilation></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TieredCompilationPolicy> logger = new();
        TieredCompilationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTieredCompilationPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TieredCompilationPolicy> logger = new();
        TieredCompilationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTieredCompilationPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TieredCompilationPolicy> logger = new();
        TieredCompilationPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ValidateExecutableReferencesMatchSelfContainedPolicy
    // CanCheck = IsPublishable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenValidateExecutableReferencesMatchSelfContainedPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable><ValidateExecutableReferencesMatchSelfContained>true</ValidateExecutableReferencesMatchSelfContained></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger = new();
        ValidateExecutableReferencesMatchSelfContainedPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenValidateExecutableReferencesMatchSelfContainedPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger = new();
        ValidateExecutableReferencesMatchSelfContainedPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenValidateExecutableReferencesMatchSelfContainedPolicyCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger = new();
        ValidateExecutableReferencesMatchSelfContainedPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustEnableNullable
    // CanCheck = repositorySettings.IsNullableGloballyEnforced
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustEnableNullablePropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Nullable>enable</Nullable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.IsNullableGloballyEnforced.Returns(true);

        CapturingLogger<MustEnableNullable> logger = new();
        MustEnableNullable check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustEnableNullablePropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.IsNullableGloballyEnforced.Returns(true);

        CapturingLogger<MustEnableNullable> logger = new();
        MustEnableNullable check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustEnableNullableCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.IsNullableGloballyEnforced.Returns(false);

        CapturingLogger<MustEnableNullable> logger = new();
        MustEnableNullable check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy
    // CanCheck = OutputType=="Exe" && IsPublishable()
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenPublishableProjectsShouldEnableRequestDelegateGeneratorPolicyPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable><OutputType>Exe</OutputType><EnableRequestDelegateGenerator>true</EnableRequestDelegateGenerator></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy> logger = new();
        PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableProjectsShouldEnableRequestDelegateGeneratorPolicyPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy> logger = new();
        PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableProjectsShouldEnableRequestDelegateGeneratorPolicyCanCheckIsFalseBecauseNotExeNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPublishable>true</IsPublishable><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy> logger = new();
        PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
