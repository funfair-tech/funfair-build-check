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
    [InlineData("CodeAnalysisTreatWarningsAsErrors", "true", "false")]
    [InlineData("DebuggerSupport", "true", "false")]
    [InlineData("EnableMicrosoftExtensionsConfigurationBinderSourceGenerator", "true", "false")]
    [InlineData("EnableNETAnalyzers", "true", "false")]
    [InlineData("EnablePackageValidation", "true", "false")]
    [InlineData("EnforceCodeStyleInBuild", "true", "false")]
    [InlineData("GenerateNeutralResourcesLanguageAttribute", "true", "false")]
    [InlineData("GenerateSBOM", "true", "false")]
    [InlineData("IlcGenerateStackTraceData", "false", "true")]
    [InlineData("IlcOptimizationPreference", "Size", "Speed")]
    [InlineData("ImplicitUsings", "disable", "enable")]
    [InlineData("IncludeSymbols", "true", "false")]
    [InlineData("JsonSerializerIsReflectionEnabledByDefault", "false", "true")]
    [InlineData("LangVersion", "latest", "9.0")]
    [InlineData("Nullable", "enable", "disable")]
    [InlineData("Features", "strict;flow-analysis", "wrong")]
    [InlineData("NuGetAudit", "true", "false")]
    [InlineData("DisableImplicitNuGetFallbackFolder", "true", "false")]
    [InlineData("OptimizationPreference", "speed", "size")]
    [InlineData("EnableRequestDelegateGenerator", "true", "false")]
    [InlineData("RunAOTCompilation", "false", "true")]
    [InlineData("SuppressTrimAnalysisWarnings", "false", "true")]
    [InlineData("IsTestingPlatformApplication", "false", "true")]
    [InlineData("IsTestProject", "false", "true")]
    [InlineData("TestingPlatformDotnetTestSupport", "false", "true")]
    [InlineData("TieredCompilation", "true", "false")]
    [InlineData("TieredPGO", "true", "false")]
    [InlineData("UseSystemResourceKeys", "true", "false")]
    [InlineData("ValidateExecutableReferencesMatchSelfContained", "true", "false")]
    [InlineData("OutputType", "Exe", "Library")]
    [InlineData("UseMicrosoftTestingPlatformRunner", "true", "false")]
    public async Task WhenPropertyHasWrongValueErrorIsLoggedAsync(
        string propertyName,
        string requiredValue,
        string wrongValue
    )
    {
        XmlDocument doc = new();
        doc.LoadXml(
            $"<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><{propertyName}>{wrongValue}</{propertyName}></PropertyGroup></Project>"
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

    [Fact]
    public async Task WhenCanCheckReturnsFalseNoErrorIsLoggedEvenIfPropertyIsAbsentAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger logger = new();
        SimplePropertyProjectCheckBase check = new(
            propertyName: "SomeProperty",
            requiredValue: "someValue",
            canCheck: static _ => false,
            logger: logger
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
