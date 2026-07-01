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

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldBeAnExecutableIsTestProjectFalseCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        // IsTestProject=false explicitly → CanCheck returns false even though xunit.v3 is referenced
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
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

    [Fact]
    public async Task WhenXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerIsTestProjectFalseCanCheckIsFalseNoErrorIsLoggedAsync()
    {
        // IsTestProject=false explicitly → CanCheck returns false even though xunit.v3 is referenced
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsTestProject>false</IsTestProject></PropertyGroup><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateJsonSerializerIsReflectionEnabledByDefaultCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateJsonSerializerIsReflectionEnabledByDefaultCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateJsonSerializerIsReflectionEnabledByDefaultCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorCheck(
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
        SimplePropertyProjectCheckBase check = CreateEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorCheck(
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
        SimplePropertyProjectCheckBase check = CreateEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorCheck(
            logger
        );

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateEnablePackageValidationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateEnablePackageValidationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateEnablePackageValidationCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateGenerateSbomCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateGenerateSbomCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateGenerateSbomCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateOptimizationPreferenceCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateOptimizationPreferenceCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateOptimizationPreferenceCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateTieredCompilationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTieredCompilationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTieredCompilationCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateValidateExecutableReferencesMatchSelfContainedCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateValidateExecutableReferencesMatchSelfContainedCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateValidateExecutableReferencesMatchSelfContainedCheck(logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateMustEnableNullableCheck(repositorySettings, logger);

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
        SimplePropertyProjectCheckBase check = CreateMustEnableNullableCheck(repositorySettings, logger);

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
        SimplePropertyProjectCheckBase check = CreateMustEnableNullableCheck(repositorySettings, logger);

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

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreatePublishableProjectsShouldEnableRequestDelegateGeneratorCheck(
            logger
        );

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
        SimplePropertyProjectCheckBase check = CreatePublishableProjectsShouldEnableRequestDelegateGeneratorCheck(
            logger
        );

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
        SimplePropertyProjectCheckBase check = CreatePublishableProjectsShouldEnableRequestDelegateGeneratorCheck(
            logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarnessExeProjectsMustSetIsTestProjectToFalse
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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestProjectCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestProjectCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestProjectCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestProjectCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarnessExeProjectsMustSetTestingPlatformDotnetTestSupportToFalse
    // CanCheck = name ends with .TestHarness && OutputType==Exe
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
        SimplePropertyProjectCheckBase check = CreateTestHarnessTestingPlatformDotnetTestSupportCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessTestingPlatformDotnetTestSupportCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessTestingPlatformDotnetTestSupportCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessTestingPlatformDotnetTestSupportCheck(logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TestHarnessExeProjectsMustSetIsTestingPlatformApplicationToFalse
    // CanCheck = name ends with .TestHarness && OutputType==Exe
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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestingPlatformApplicationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestingPlatformApplicationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestingPlatformApplicationCheck(logger);

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
        SimplePropertyProjectCheckBase check = CreateTestHarnessIsTestingPlatformApplicationCheck(logger);

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
    // SuppressTrimAnalysisWarningsMustBeFalsePolicy
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

    private static SimplePropertyProjectCheckBase CreateEnableMicrosoftExtensionsConfigurationBinderSourceGeneratorCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPackable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateEnablePackageValidationCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "EnablePackageValidation",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPackable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateGenerateSbomCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "GenerateSBOM",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPackable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateIncludeSymbolsCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "IncludeSymbols",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPackable(project) && !IsDotNetTool(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateJsonSerializerIsReflectionEnabledByDefaultCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "JsonSerializerIsReflectionEnabledByDefault",
            requiredValue: "false",
            logger: logger,
            canCheck: static project => IsPackable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateMustEnableNullableCheck(
        IRepositorySettings repositorySettings,
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "Nullable",
            requiredValue: "enable",
            logger: logger,
            canCheck: _ => repositorySettings.IsNullableGloballyEnforced
        );
    }

    private static SimplePropertyProjectCheckBase CreateOptimizationPreferenceCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "OptimizationPreference",
            requiredValue: "speed",
            logger: logger,
            canCheck: static project => IsPackable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateTieredCompilationCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "TieredCompilation",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPublishable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateValidateExecutableReferencesMatchSelfContainedCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "ValidateExecutableReferencesMatchSelfContained",
            requiredValue: "true",
            logger: logger,
            canCheck: static project => IsPublishable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreatePublishableProjectsShouldEnableRequestDelegateGeneratorCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "EnableRequestDelegateGenerator",
            requiredValue: "true",
            logger: logger,
            canCheck: static project =>
                StringComparer.OrdinalIgnoreCase.Equals(GetOutputType(project), "Exe") && IsPublishable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateTestHarnessIsTestProjectCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "IsTestProject",
            requiredValue: "false",
            logger: logger,
            canCheck: static project => IsTestHarnessExecutable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateTestHarnessTestingPlatformDotnetTestSupportCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "TestingPlatformDotnetTestSupport",
            requiredValue: "false",
            logger: logger,
            canCheck: static project => IsTestHarnessExecutable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateTestHarnessIsTestingPlatformApplicationCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "IsTestingPlatformApplication",
            requiredValue: "false",
            logger: logger,
            canCheck: static project => IsTestHarnessExecutable(project)
        );
    }

    private static SimplePropertyProjectCheckBase CreateXUnitV3ProjectsShouldBeAnExecutableCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "OutputType",
            requiredValue: "Exe",
            logger: logger,
            canCheck: static project =>
                !IsExplicitlyNotTestProject(project) && IsTestProject(project) && ReferencesPackage(project, "xunit.v3")
        );
    }

    private static SimplePropertyProjectCheckBase CreateXUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunnerCheck(
        CapturingLogger logger
    )
    {
        return CreateSimpleCheck(
            propertyName: "UseMicrosoftTestingPlatformRunner",
            requiredValue: "true",
            logger: logger,
            canCheck: static project =>
                !IsExplicitlyNotTestProject(project)
                && IsTestProject(project)
                && (ReferencesPackage(project, "xunit.v3") || ReferencesPackage(project, "xunit.v3.extensibility.core"))
        );
    }

    private static SimplePropertyProjectCheckBase CreateSuppressTrimAnalysisWarningsCheck(CapturingLogger logger)
    {
        return CreateSimpleCheck(
            propertyName: "SuppressTrimAnalysisWarnings",
            requiredValue: "false",
            logger: logger,
            canCheck: static project => HasProperty(project, "SuppressTrimAnalysisWarnings")
        );
    }

    private static SimplePropertyProjectCheckBase CreateSimpleCheck(
        string propertyName,
        string requiredValue,
        CapturingLogger logger,
        Func<ProjectContext, bool>? canCheck = null
    )
    {
        return new(propertyName: propertyName, requiredValue: requiredValue, canCheck: canCheck, logger: logger);
    }

    private static string GetOutputType(in ProjectContext project)
    {
        XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/OutputType");

        return node is null || string.IsNullOrWhiteSpace(node.InnerText) ? "Library" : node.InnerText;
    }

    private static bool HasProperty(in ProjectContext project, string propertyName)
    {
        XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/" + propertyName);

        return node is not null && !string.IsNullOrWhiteSpace(node.InnerText);
    }

    private static bool IsDotNetTool(in ProjectContext project)
    {
        return IsTrue(project, "PackAsTool", defaultValue: false);
    }

    private static bool IsPackable(in ProjectContext project)
    {
        return IsTrue(project, "IsPackable", defaultValue: true);
    }

    private static bool IsPublishable(in ProjectContext project)
    {
        return IsTrue(project, "IsPublishable", defaultValue: true);
    }

    private static bool IsTestHarnessExecutable(in ProjectContext project)
    {
        return project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(GetOutputType(project), "Exe");
    }

    private static bool IsTestProject(in ProjectContext project)
    {
        return ReferencesPackage(project, "xunit", "xunit.v3", "NSubstitute", "FunFair.Test.Common");
    }

    private static bool IsExplicitlyNotTestProject(in ProjectContext project)
    {
        XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/IsTestProject");

        return node is not null && StringComparer.OrdinalIgnoreCase.Equals(node.InnerText, "false");
    }

    private static bool IsTrue(in ProjectContext project, string propertyName, bool defaultValue)
    {
        XmlNode? node = project.CsProjXml.SelectSingleNode("/Project/PropertyGroup/" + propertyName);

        if (node is null || string.IsNullOrWhiteSpace(node.InnerText))
        {
            return defaultValue;
        }

        return StringComparer.OrdinalIgnoreCase.Equals(node.InnerText, "true");
    }

    private static bool ReferencesPackage(in ProjectContext project, params string[] packageNames)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes("/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return false;
        }

        foreach (XmlNode node in nodes)
        {
            if (node is not XmlElement element)
            {
                continue;
            }

            string packageName = element.GetAttribute("Include");

            foreach (string candidate in packageNames)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(candidate, packageName))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
