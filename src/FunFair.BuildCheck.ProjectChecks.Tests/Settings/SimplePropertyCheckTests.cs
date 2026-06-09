using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class SimplePropertyCheckTests : TestBase
{
    [Theory]
    [InlineData("AnalysisLevel", "latest")]
    [InlineData("AnalysisMode", "AllEnabledByDefault")]
    [InlineData("CodeAnalysisTreatWarningsAsErrors", "true")]
    [InlineData("DebuggerSupport", "true")]
    [InlineData("EnableMicrosoftExtensionsConfigurationBinderSourceGenerator", "true")]
    [InlineData("EnableNETAnalyzers", "true")]
    [InlineData("EnablePackageValidation", "true")]
    [InlineData("EnforceCodeStyleInBuild", "true")]
    [InlineData("GenerateNeutralResourcesLanguageAttribute", "true")]
    [InlineData("GenerateSBOM", "true")]
    [InlineData("IlcGenerateStackTraceData", "false")]
    [InlineData("IlcOptimizationPreference", "Size")]
    [InlineData("ImplicitUsings", "disable")]
    [InlineData("IncludeSymbols", "true")]
    [InlineData("JsonSerializerIsReflectionEnabledByDefault", "false")]
    [InlineData("LangVersion", "latest")]
    [InlineData("Nullable", "enable")]
    [InlineData("Features", "strict;flow-analysis")]
    [InlineData("NuGetAudit", "true")]
    [InlineData("DisableImplicitNuGetFallbackFolder", "true")]
    [InlineData("OptimizationPreference", "speed")]
    [InlineData("EnableRequestDelegateGenerator", "true")]
    [InlineData("RunAOTCompilation", "false")]
    [InlineData("SuppressTrimAnalysisWarnings", "false")]
    [InlineData("IsTestingPlatformApplication", "false")]
    [InlineData("IsTestProject", "false")]
    [InlineData("TestingPlatformDotnetTestSupport", "false")]
    [InlineData("TieredCompilation", "true")]
    [InlineData("TieredPGO", "true")]
    [InlineData("UseSystemResourceKeys", "true")]
    [InlineData("ValidateExecutableReferencesMatchSelfContained", "true")]
    [InlineData("OutputType", "Exe")]
    [InlineData("UseMicrosoftTestingPlatformRunner", "true")]
    public async Task WhenPropertyIsCorrectNoErrorIsLoggedAsync(string propertyName, string requiredValue)
    {
        XmlDocument doc = new();
        doc.LoadXml(
            $"<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><{propertyName}>{requiredValue}</{propertyName}></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = new(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: null,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Theory]
    [InlineData("AnalysisLevel", "latest")]
    [InlineData("AnalysisMode", "AllEnabledByDefault")]
    [InlineData("CodeAnalysisTreatWarningsAsErrors", "true")]
    [InlineData("DebuggerSupport", "true")]
    [InlineData("EnableMicrosoftExtensionsConfigurationBinderSourceGenerator", "true")]
    [InlineData("EnableNETAnalyzers", "true")]
    [InlineData("EnablePackageValidation", "true")]
    [InlineData("EnforceCodeStyleInBuild", "true")]
    [InlineData("GenerateNeutralResourcesLanguageAttribute", "true")]
    [InlineData("GenerateSBOM", "true")]
    [InlineData("IlcGenerateStackTraceData", "false")]
    [InlineData("IlcOptimizationPreference", "Size")]
    [InlineData("ImplicitUsings", "disable")]
    [InlineData("IncludeSymbols", "true")]
    [InlineData("JsonSerializerIsReflectionEnabledByDefault", "false")]
    [InlineData("LangVersion", "latest")]
    [InlineData("Nullable", "enable")]
    [InlineData("Features", "strict;flow-analysis")]
    [InlineData("NuGetAudit", "true")]
    [InlineData("DisableImplicitNuGetFallbackFolder", "true")]
    [InlineData("OptimizationPreference", "speed")]
    [InlineData("EnableRequestDelegateGenerator", "true")]
    [InlineData("RunAOTCompilation", "false")]
    [InlineData("SuppressTrimAnalysisWarnings", "false")]
    [InlineData("IsTestingPlatformApplication", "false")]
    [InlineData("IsTestProject", "false")]
    [InlineData("TestingPlatformDotnetTestSupport", "false")]
    [InlineData("TieredCompilation", "true")]
    [InlineData("TieredPGO", "true")]
    [InlineData("UseSystemResourceKeys", "true")]
    [InlineData("ValidateExecutableReferencesMatchSelfContained", "true")]
    [InlineData("OutputType", "Exe")]
    [InlineData("UseMicrosoftTestingPlatformRunner", "true")]
    public async Task WhenPropertyIsAbsentErrorIsLoggedAsync(string propertyName, string requiredValue)
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = new(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: null,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Theory]
    [InlineData("AnalysisLevel", "latest", "wrong")]
    [InlineData("AnalysisMode", "AllEnabledByDefault", "wrong")]
    [InlineData("CodeAnalysisTreatWarningsAsErrors", "true", "wrong")]
    [InlineData("DebuggerSupport", "true", "wrong")]
    [InlineData("EnableMicrosoftExtensionsConfigurationBinderSourceGenerator", "true", "wrong")]
    [InlineData("EnableNETAnalyzers", "true", "wrong")]
    [InlineData("EnablePackageValidation", "true", "wrong")]
    [InlineData("EnforceCodeStyleInBuild", "true", "wrong")]
    [InlineData("GenerateNeutralResourcesLanguageAttribute", "true", "wrong")]
    [InlineData("GenerateSBOM", "true", "wrong")]
    [InlineData("IlcGenerateStackTraceData", "false", "wrong")]
    [InlineData("IlcOptimizationPreference", "Size", "wrong")]
    [InlineData("ImplicitUsings", "disable", "wrong")]
    [InlineData("IncludeSymbols", "true", "wrong")]
    [InlineData("JsonSerializerIsReflectionEnabledByDefault", "false", "wrong")]
    [InlineData("LangVersion", "latest", "wrong")]
    [InlineData("Nullable", "enable", "wrong")]
    [InlineData("Features", "strict;flow-analysis", "wrong")]
    [InlineData("NuGetAudit", "true", "wrong")]
    [InlineData("DisableImplicitNuGetFallbackFolder", "true", "wrong")]
    [InlineData("OptimizationPreference", "speed", "wrong")]
    [InlineData("EnableRequestDelegateGenerator", "true", "wrong")]
    [InlineData("RunAOTCompilation", "false", "wrong")]
    [InlineData("SuppressTrimAnalysisWarnings", "false", "wrong")]
    [InlineData("IsTestingPlatformApplication", "false", "wrong")]
    [InlineData("IsTestProject", "false", "wrong")]
    [InlineData("TestingPlatformDotnetTestSupport", "false", "wrong")]
    [InlineData("TieredCompilation", "true", "wrong")]
    [InlineData("TieredPGO", "true", "wrong")]
    [InlineData("UseSystemResourceKeys", "true", "wrong")]
    [InlineData("ValidateExecutableReferencesMatchSelfContained", "true", "wrong")]
    [InlineData("OutputType", "Exe", "Library")]
    [InlineData("UseMicrosoftTestingPlatformRunner", "true", "wrong")]
    public async Task WhenPropertyHasWrongValueErrorIsLoggedAsync(
        string propertyName,
        string requiredValue,
        string actualValue
    )
    {
        XmlDocument doc = new();
        doc.LoadXml(
            $"<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><{propertyName}>{actualValue}</{propertyName}></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = new(
            propertyName: propertyName,
            requiredValue: requiredValue,
            canCheck: null,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
