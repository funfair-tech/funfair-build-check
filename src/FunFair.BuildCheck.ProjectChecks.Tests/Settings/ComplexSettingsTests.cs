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

public sealed class ComplexSettingsTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // NoDuplicateProjectSettings
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsProjectHasNoPropertiesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsProjectHasUniquePropertiesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Foo>bar</Foo><Baz>qux</Baz></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsProjectHasDuplicatePropertyErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Foo>bar</Foo><Foo>baz</Foo></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsPropertyGroupHasConditionItIsSkippedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup Condition=\"'$(Config)'=='Debug'\"><Foo>bar</Foo><Foo>baz</Foo></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsPropertyWithConditionIsSkippedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Foo Condition=\"'$(Config)'=='Debug'\">bar</Foo><Foo Condition=\"'$(Config)'=='Release'\">baz</Foo></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsDuplicatePropertyIncludesSelfReferenceNoErrorIsLoggedAsync()
    {
        // When a duplicate references itself via $(PropertyName), it is allowed
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Foo>bar</Foo><Foo>$(Foo);baz</Foo></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenNoDuplicateProjectSettingsPropertyAppearsInMultipleCasesErrorIsLoggedAsync()
    {
        // Same property name in different cases (e.g., Foo vs FOO)
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Foo>bar</Foo><FOO>baz</FOO></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoDuplicateProjectSettings> logger = new();
        NoDuplicateProjectSettings check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DoesNotUseRootNamespace
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDoesNotUseRootNamespaceProjectHasNoRootNamespaceNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotUseRootNamespace> logger = new();
        DoesNotUseRootNamespace check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDoesNotUseRootNamespaceProjectHasRootNamespaceErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><RootNamespace>My.Namespace</RootNamespace></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotUseRootNamespace> logger = new();
        DoesNotUseRootNamespace check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustSpecifyOutputType
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustSpecifyOutputTypeProjectHasExeOutputTypeNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustSpecifyOutputType> logger = new();
        MustSpecifyOutputType check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustSpecifyOutputTypeProjectHasLibraryOutputTypeNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustSpecifyOutputType> logger = new();
        MustSpecifyOutputType check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustSpecifyOutputTypeProjectHasNoOutputTypeErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustSpecifyOutputType> logger = new();
        MustSpecifyOutputType check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustSpecifyOutputTypeProjectHasInvalidOutputTypeErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>WinExe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustSpecifyOutputType> logger = new();
        MustSpecifyOutputType check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // DotNetToolsMustBePublishablePolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDotNetToolsMustBePublishablePolicyProjectIsNotToolNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotNetToolsMustBePublishablePolicyToolProjectIsPublishableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>true</PackAsTool><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDotNetToolsMustBePublishablePolicyToolProjectIsNotPublishableErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackAsTool>true</PackAsTool><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ProjectsShouldHaveTrimAnalyzerConfiguredPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenProjectsShouldHaveTrimAnalyzerConfiguredPolicyTrimAnalyzerIsTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><EnableTrimAnalyzer>true</EnableTrimAnalyzer></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger = new();
        ProjectsShouldHaveTrimAnalyzerConfiguredPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectsShouldHaveTrimAnalyzerConfiguredPolicyTrimAnalyzerIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><EnableTrimAnalyzer>false</EnableTrimAnalyzer></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger = new();
        ProjectsShouldHaveTrimAnalyzerConfiguredPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectsShouldHaveTrimAnalyzerConfiguredPolicyTrimAnalyzerIsMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger = new();
        ProjectsShouldHaveTrimAnalyzerConfiguredPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectsShouldHaveTrimAnalyzerConfiguredPolicyTrimAnalyzerIsInvalidErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><EnableTrimAnalyzer>maybe</EnableTrimAnalyzer></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger = new();
        ProjectsShouldHaveTrimAnalyzerConfiguredPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectsShouldHaveTrimAnalyzerConfiguredPolicyProjectIsTrimmableAndTrimAnalyzerIsFalseErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><EnableTrimAnalyzer>false</EnableTrimAnalyzer><IsTrimmable>true</IsTrimmable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger = new();
        ProjectsShouldHaveTrimAnalyzerConfiguredPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotSpecifyBothPublishAotAndPublishReadyToRunPolicyNeitherIsSetNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotSpecifyBothPublishAotAndPublishReadyToRunPolicyOnlyAotIsSetNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PublishAot>true</PublishAot></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotSpecifyBothPublishAotAndPublishReadyToRunPolicyBothAreSetErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PublishAot>true</PublishAot><PublishReadyToRun>true</PublishReadyToRun></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // PublishableExesMustHavePublishAotSetPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenPublishableExesMustHavePublishAotSetPolicyProjectIsLibraryNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHavePublishAotSetPolicyProjectIsExeButNotPublishableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHavePublishAotSetPolicyPublishableExeHasNoPublishAotErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHavePublishAotSetPolicyPublishAotTrueButNotTrimmableErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable><PublishAot>true</PublishAot></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHavePublishAotSetPolicyPublishAotTrueAndTrimmableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable><PublishAot>true</PublishAot><IsTrimmable>true</IsTrimmable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // PublishableExesMustHaveRuntimeIdentifiers
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenPublishableExesMustHaveRuntimeIdentifiersProjectIsLibraryNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHaveRuntimeIdentifiers> logger = new();
        PublishableExesMustHaveRuntimeIdentifiers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHaveRuntimeIdentifiersExeNotPublishableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>false</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHaveRuntimeIdentifiers> logger = new();
        PublishableExesMustHaveRuntimeIdentifiers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHaveRuntimeIdentifiersPublishableExeWithNoRidErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHaveRuntimeIdentifiers> logger = new();
        PublishableExesMustHaveRuntimeIdentifiers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPublishableExesMustHaveRuntimeIdentifiersPublishableExeWithRidNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><IsPublishable>true</IsPublishable><RuntimeIdentifiers>linux-x64;win-x64</RuntimeIdentifiers></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PublishableExesMustHaveRuntimeIdentifiers> logger = new();
        PublishableExesMustHaveRuntimeIdentifiers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenIsTransformWebConfigDisabledProjectIsNotWebSdkNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger = new();
        IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenIsTransformWebConfigDisabledProjectIsWebSdkButNotLibraryNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk.Web\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger = new();
        IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenIsTransformWebConfigDisabledProjectIsWebLibraryWithPropertyTrueNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk.Web\"><PropertyGroup><OutputType>Library</OutputType><IsTransformWebConfigDisabled>true</IsTransformWebConfigDisabled></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger = new();
        IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenIsTransformWebConfigDisabledProjectIsWebLibraryWithoutPropertyErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk.Web\"><PropertyGroup><OutputType>Library</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger = new();
        IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustEnableStrictMode
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustEnableStrictModePropertyIsCorrectNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Features>strict;flow-analysis</Features></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSimpleCheck("Features", "strict;flow-analysis", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustEnableStrictModePropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSimpleCheck("Features", "strict;flow-analysis", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustEnableStrictModePropertyHasWrongValueErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Features>strict</Features></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = CreateSimpleCheck("Features", "strict;flow-analysis", logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    private static SimplePropertyProjectCheckBase CreateSimpleCheck(
        string propertyName,
        string requiredValue,
        CapturingLogger logger
    )
    {
        return new(propertyName: propertyName, requiredValue: requiredValue, canCheck: null, logger: logger);
    }
}
