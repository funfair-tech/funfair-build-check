using System;
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
    // XUnitV3ProjectsShouldBeAnExecutable (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldBeAnExecutableCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldBeAnExecutableCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldBeAnExecutableCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport
    // IsTestProject=false → must not define
    // IsTestProject (via packages) && xunit.v3 → must be true
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
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportNoPackagesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportViaExtensibilityCorePropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3.extensibility.core\" Version=\"1.0.0\" /><PackageReference Include=\"NSubstitute\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportIsTestProjectFalseAndPropertyAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportIsTestProjectFalseAndPropertyDefinedErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject><TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupportIsTestProjectFalseWithIsTestingPlatformApplicationFalseAndPropertyDefinedNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject><IsTestingPlatformApplication>false</IsTestingPlatformApplication><TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCheck(
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCheck(
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCheck(
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineIsTestingPlatformApplication
    // IsTestProject=false → must not define
    // IsTestProject (via packages) && xunit.v3 → must be true
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationPropertyIsCorrectAndCanCheckIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestingPlatformApplication>true</IsTestingPlatformApplication></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationPropertyIsAbsentAndCanCheckIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationNoPackagesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationViaExtensibilityCorePropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3.extensibility.core\" Version=\"1.0.0\" /><PackageReference Include=\"NSubstitute\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationIsTestProjectFalseAndPropertyAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationIsTestProjectFalseAndPropertyDefinedErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject><IsTestingPlatformApplication>true</IsTestingPlatformApplication></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineIsTestingPlatformApplicationIsTestProjectFalseAndPropertyDefinedAsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject><IsTestingPlatformApplication>false</IsTestingPlatformApplication></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // JsonSerializerIsReflectionEnabledByDefault (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "JsonSerializerIsReflectionEnabledByDefault",
            "false",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "JsonSerializerIsReflectionEnabledByDefault",
            "false",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "JsonSerializerIsReflectionEnabledByDefault",
            "false",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // EnableMicrosoftExtensionsConfigurationBinderSourceGenerator (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
            "true",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
            "true",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck(
            "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
            "true",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // EnablePackageValidation (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("EnablePackageValidation", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("EnablePackageValidation", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("EnablePackageValidation", "true", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // GenerateSBOM (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("GenerateSBOM", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("GenerateSBOM", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("GenerateSBOM", "true", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // IncludeSymbols (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIncludeSymbolsCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIncludeSymbolsCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIncludeSymbolsCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // OptimizationPreference (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("OptimizationPreference", "speed", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("OptimizationPreference", "speed", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPackableCheck("OptimizationPreference", "speed", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TieredCompilation (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck("TieredCompilation", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck("TieredCompilation", "true", logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck("TieredCompilation", "true", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ValidateExecutableReferencesMatchSelfContained (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck(
            "ValidateExecutableReferencesMatchSelfContained",
            "true",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck(
            "ValidateExecutableReferencesMatchSelfContained",
            "true",
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateIsPublishableCheck(
            "ValidateExecutableReferencesMatchSelfContained",
            "true",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // Nullable (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateNullableCheck(repositorySettings, logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateNullableCheck(repositorySettings, logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateNullableCheck(repositorySettings, logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // EnableRequestDelegateGenerator (via factory)
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateEnableRequestDelegateGeneratorCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateEnableRequestDelegateGeneratorCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateEnableRequestDelegateGeneratorCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarness IsTestProject=false (via factory)
    // CanCheck = name ends with .TestHarness && OutputType==Exe
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTestHarnessExeProjectHasIsTestProjectFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsTestProject>false</IsTestProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestProject", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessExeProjectMissingIsTestProjectErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestProject", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNonTestHarnessProjectIsSkippedByIsTestProjectCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestProject", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessLibraryProjectIsSkippedByIsTestProjectCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestProject", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarness TestingPlatformDotnetTestSupport=false (via factory)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTestHarnessExeProjectHasTestingPlatformDotnetTestSupportFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><TestingPlatformDotnetTestSupport>false</TestingPlatformDotnetTestSupport></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck(
            "TestingPlatformDotnetTestSupport",
            "false",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessExeProjectMissingTestingPlatformDotnetTestSupportErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck(
            "TestingPlatformDotnetTestSupport",
            "false",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNonTestHarnessProjectIsSkippedByTestingPlatformDotnetTestSupportCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck(
            "TestingPlatformDotnetTestSupport",
            "false",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessLibraryProjectIsSkippedByTestingPlatformDotnetTestSupportCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck(
            "TestingPlatformDotnetTestSupport",
            "false",
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarness IsTestingPlatformApplication=false (via factory)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTestHarnessExeProjectHasIsTestingPlatformApplicationFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsTestingPlatformApplication>false</IsTestingPlatformApplication></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestingPlatformApplication", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessExeProjectMissingIsTestingPlatformApplicationErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestingPlatformApplication", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNonTestHarnessProjectIsSkippedByIsTestingPlatformApplicationCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestingPlatformApplication", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTestHarnessLibraryProjectIsSkippedByIsTestingPlatformApplicationCheckNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTestHarnessCheck("IsTestingPlatformApplication", "false", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport
    // TestHarness Exe projects are excluded from this check
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTestHarnessExeProjectWithIsTestProjectFalseAndPropertyDefinedNoErrorIsLoggedByXUnitCheckAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsTestProject>false</IsTestProject><TestingPlatformDotnetTestSupport>false</TestingPlatformDotnetTestSupport></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger = new();
        XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XUnitV3ProjectsShouldDefineIsTestingPlatformApplication
    // TestHarness Exe projects are excluded from this check
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTestHarnessExeProjectWithIsTestProjectFalseAndIsTestingPlatformApplicationDefinedNoErrorIsLoggedByXUnitCheckAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsTestProject>false</IsTestProject><IsTestingPlatformApplication>false</IsTestingPlatformApplication></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "MyProject.TestHarness", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger = new();
        XUnitV3ProjectsShouldDefineIsTestingPlatformApplication check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // SuppressTrimAnalysisWarnings (via factory)
    // CanCheck = project.HasProperty("SuppressTrimAnalysisWarnings")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenSuppressTrimAnalysisWarningsMustBeFalsePolicyPropertyIsAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSuppressTrimAnalysisWarningsCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenSuppressTrimAnalysisWarningsMustBeFalsePolicyPropertyIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><SuppressTrimAnalysisWarnings>false</SuppressTrimAnalysisWarnings></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSuppressTrimAnalysisWarningsCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenSuppressTrimAnalysisWarningsMustBeFalsePolicyPropertyIsTrueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSuppressTrimAnalysisWarningsCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // Factory helpers
    // ──────────────────────────────────────────────────────────────

    private static SimplePropertyProjectCheckBase CreateXUnitV3ProjectsShouldBeAnExecutableCheck(CapturingLogger logger)
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "OutputType",
            requiredValue: "Exe",
            canCheck: project => IsTestProject(project) && ReferencesPackage(project, "xunit.v3"),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCheck(
        CapturingLogger logger
    )
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "UseMicrosoftTestingPlatformRunner",
            requiredValue: "true",
            canCheck: project =>
                IsTestProject(project)
                && (
                    ReferencesPackage(project, "xunit.v3") || ReferencesPackage(project, "xunit.v3.extensibility.core")
                ),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateIsPackableCheck(
        string propertyName,
        string requiredValue,
        CapturingLogger logger
    )
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: static project => IsPackable(project),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateIncludeSymbolsCheck(CapturingLogger logger)
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "IncludeSymbols",
            requiredValue: "true",
            canCheck: static project => IsPackable(project) && !IsDotNetTool(project),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateIsPublishableCheck(
        string propertyName,
        string requiredValue,
        CapturingLogger logger
    )
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: static project => IsPublishable(project),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateNullableCheck(
        IRepositorySettings repositorySettings,
        CapturingLogger logger
    )
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "Nullable",
            requiredValue: "enable",
            canCheck: _ => repositorySettings.IsNullableGloballyEnforced,
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateEnableRequestDelegateGeneratorCheck(CapturingLogger logger)
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "EnableRequestDelegateGenerator",
            requiredValue: "true",
            canCheck: static project =>
                StringComparer.OrdinalIgnoreCase.Equals(GetOutputType(project), y: "Exe") && IsPublishable(project),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateTestHarnessCheck(
        string propertyName,
        string requiredValue,
        CapturingLogger logger
    )
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: static project => IsTestHarnessExecutable(project),
            logger: logger
        );
    }

    private static SimplePropertyProjectCheckBase CreateSuppressTrimAnalysisWarningsCheck(CapturingLogger logger)
    {
        return new SimplePropertyProjectCheckBase(
            propertyName: "SuppressTrimAnalysisWarnings",
            requiredValue: "false",
            canCheck: static project => HasProperty(project, "SuppressTrimAnalysisWarnings"),
            logger: logger
        );
    }

    // ──────────────────────────────────────────────────────────────
    // XPath-based helpers used by factory methods (no ILogger needed)
    // ──────────────────────────────────────────────────────────────

    private static bool IsTestProject(in ProjectContext project)
    {
        return ReferencesPackage(project, "xunit")
            || ReferencesPackage(project, "xunit.v3")
            || ReferencesPackage(project, "NSubstitute")
            || ReferencesPackage(project, "FunFair.Test.Common");
    }

    private static bool ReferencesPackage(in ProjectContext project, string packageName)
    {
        System.Xml.XmlNodeList? nodes = project.CsProjXml.SelectNodes("/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return false;
        }

        foreach (System.Xml.XmlElement node in nodes)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(node.GetAttribute("Include"), y: packageName))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPackable(in ProjectContext project)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(
            GetStringProperty(project, "/Project/PropertyGroup/IsPackable", "true"),
            y: "true"
        );
    }

    private static bool IsDotNetTool(in ProjectContext project)
    {
        System.Xml.XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/PackAsTool");

        return node is not null
            && !string.IsNullOrWhiteSpace(node.InnerText)
            && StringComparer.OrdinalIgnoreCase.Equals(node.InnerText, y: "True");
    }

    private static bool IsPublishable(in ProjectContext project)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(
            GetStringProperty(project, "/Project/PropertyGroup/IsPublishable", "true"),
            y: "true"
        );
    }

    private static string GetOutputType(in ProjectContext project)
    {
        return GetStringProperty(project, "/Project/PropertyGroup/OutputType", "Library");
    }

    private static bool IsTestHarnessExecutable(in ProjectContext project)
    {
        return project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(GetOutputType(project), y: "Exe");
    }

    private static bool HasProperty(in ProjectContext project, string propertyName)
    {
        System.Xml.XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/" + propertyName);

        return node is not null;
    }

    private static string GetStringProperty(in ProjectContext project, string path, string defaultValue)
    {
        System.Xml.XmlNode? node = project.CsProjXml.SelectSingleNode(path);

        if (node is not null && !string.IsNullOrWhiteSpace(node.InnerText))
        {
            return node.InnerText;
        }

        return defaultValue;
    }
}
