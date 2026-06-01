using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class MustNotDefinePropertyTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // MustNotDefineCodeAnalysisRuleSet  (CodeAnalysisRuleSet must not be defined)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotDefineCodeAnalysisRuleSetPropertyIsAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDefineCodeAnalysisRuleSet> logger = new();
        MustNotDefineCodeAnalysisRuleSet check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotDefineCodeAnalysisRuleSetPropertyIsPresentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><CodeAnalysisRuleSet>rules.ruleset</CodeAnalysisRuleSet></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotDefineCodeAnalysisRuleSet> logger = new();
        MustNotDefineCodeAnalysisRuleSet check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // MustNotUseOpenApiAnalyzers  (IncludeOpenAPIAnalyzers must not be defined)
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenMustNotUseOpenApiAnalyzersPropertyIsAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseOpenApiAnalyzers> logger = new();
        MustNotUseOpenApiAnalyzers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenMustNotUseOpenApiAnalyzersPropertyIsPresentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IncludeOpenAPIAnalyzers>true</IncludeOpenAPIAnalyzers></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseOpenApiAnalyzers> logger = new();
        MustNotUseOpenApiAnalyzers check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ShouldNotRemoveFromCompilation
    // Logs an error for each <Compile Remove="..." /> item found
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenShouldNotRemoveFromCompilationNoCompileRemoveItemsExistNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldNotRemoveFromCompilation> logger = new();
        ShouldNotRemoveFromCompilation check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenShouldNotRemoveFromCompilationCompileRemoveItemExistsErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><Compile Remove=\"Models\\Foo.cs\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ShouldNotRemoveFromCompilation> logger = new();
        ShouldNotRemoveFromCompilation check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // ErrorPolicyWarningAsErrors
    // Requires WarningsAsErrors node and TreatWarningsAsErrors=true
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenErrorPolicyWarningAsErrorsHasCorrectSettingsNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><WarningsAsErrors /><TreatWarningsAsErrors>true</TreatWarningsAsErrors></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ErrorPolicyWarningAsErrors> logger = new();
        ErrorPolicyWarningAsErrors check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenErrorPolicyWarningAsErrorsTreatWarningsAsErrorsIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><WarningsAsErrors /></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ErrorPolicyWarningAsErrors> logger = new();
        ErrorPolicyWarningAsErrors check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenErrorPolicyWarningAsErrorsGlobalSettingWithConditionalGroupsHavingWarningsAsErrorsNoErrorIsLoggedAsync()
    {
        // Global WarningsAsErrors present, conditional group also has it — no error
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><WarningsAsErrors /><TreatWarningsAsErrors>true</TreatWarningsAsErrors></PropertyGroup><PropertyGroup Condition=\"'$(Config)'=='Debug'\"><WarningsAsErrors /></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ErrorPolicyWarningAsErrors> logger = new();
        ErrorPolicyWarningAsErrors check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenErrorPolicyWarningAsErrorsConditionalGroupMissingWarningsAsErrorsButGlobalPresentNoErrorIsLoggedAsync()
    {
        // No global WarningsAsErrors, conditional group is missing it — error for the configuration group
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TreatWarningsAsErrors>true</TreatWarningsAsErrors></PropertyGroup><PropertyGroup Condition=\"'$(Config)'=='Debug'\"></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ErrorPolicyWarningAsErrors> logger = new();
        ErrorPolicyWarningAsErrors check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
