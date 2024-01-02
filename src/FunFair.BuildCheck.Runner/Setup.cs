using System;
using System.Diagnostics.CodeAnalysis;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.References;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.SolutionChecks;
using Microsoft.Extensions.DependencyInjection;

namespace FunFair.BuildCheck.Runner;

internal static class Setup
{
    public static IServiceCollection SetupSolutionChecks(this IServiceCollection services)
    {
        return services.AddSolutionCheck<GlobalJsonIsLatest>()
                       .AddSolutionCheck<GlobalJsonMustSpecifyCorrectRollForwardPolicy>()
                       .AddSolutionCheck<GlobalJsonMustNotAllowPreRelease>()
                       .AddSolutionCheck<AllProjectsExist>()
                       .AddSolutionCheck<NoOrphanedProjectsExist>();
    }

    [SuppressMessage(category: "", checkId: "MA0051: Method too long", Justification = "Registering Analyzers")]
    public static IServiceCollection SetupProjectChecks(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        return services.GeneralProjectSettings()
                       .XmlDocumentationProjectSettings()
                       .ProjectReferences()
                       .ProjectIntegrityChecks()
                       .PackagingSettings()
                       .PublishingSettings()
                       .StaticCodeAnalysis(repositorySettings);
    }

    private static IServiceCollection ProjectIntegrityChecks(this IServiceCollection services)
    {
        return services.AddProjectCheck<DoesNotReferenceByDll>()
                       .AddProjectCheck<HasConsistentNuGetPackages>()
                       .AddProjectCheck<LibrariesShouldNotDependOnExecutables>()
                       .AddProjectCheck<MustNotReferenceObsoleteAspNetPackages>()
                       .AddProjectCheck<ReferencedProjectsMustExist>()
                       .AddProjectCheck<ReferencesNugetPackageOnlyOnce>()
                       .AddProjectCheck<ShouldNotRemoveFromCompilation>();
    }

    private static IServiceCollection ProjectReferences(this IServiceCollection services)
    {
        return services.AddProjectCheck<MustHaveSourceLinkPackage>()
                       .AddProjectCheck<MustNotReferenceMicrosoftVisualStudioThreading>()
                       .AddProjectCheck<ShouldNotReferenceAllMetaPackagesInPackableProjects>()
                       .AddProjectCheck<ShouldUseAbstractionsCachingPackage>()
                       .AddProjectCheck<ShouldUseAbstractionsConfigurationPackage>()
                       .AddProjectCheck<ShouldUseAbstractionsDependencyInjectionPackage>()
                       .AddProjectCheck<ShouldUseAbstractionsExtensionHostingPackage>()
                       .AddProjectCheck<ShouldUseAbstractionsFileProvidersPackage>()
                       .AddProjectCheck<ShouldUseAbstractionsLoggingPackage>()
                       .AddProjectCheck<ShouldUseNSubstituteRatherThanMoqPackage>()
                       .AddProjectCheck<UsingJwtAuthenticationMustIncludeIdentityModelJwt>();
    }

    private static IServiceCollection XmlDocumentationProjectSettings(this IServiceCollection services)
    {
        return services.AddProjectCheck<XmlDocumentationFileRequiredPolicy>()
                       .AddProjectCheck<XmlDocumentationFileProhibitedPolicy>();
    }

    [SuppressMessage(category: "Philips.CodeAnalysis.DuplicateCodeAnalyzer", checkId: "PH2071: Duplicate code shape", Justification = "Registering Analyzers")]
    private static IServiceCollection GeneralProjectSettings(this IServiceCollection services)
    {
        return services.AddProjectCheck<DoesNotUseDotNetCliToolReference>()
                       .AddProjectCheck<DoesNotUseRootNamespace>()
                       .AddProjectCheck<GenerateNeutralResourcesLanguageAttributePolicy>()
                       .AddProjectCheck<ImplicitUsingsPolicy>()
                       .AddProjectCheck<LanguagePolicyUseLatestVersion>()
                       .AddProjectCheck<LibrariesShouldBePackablePolicy>()
                       .AddProjectCheck<MustEnableNullable>()
                       .AddProjectCheck<MustNotDisableUnexpectedWarnings>()
                       .AddProjectCheck<MustSpecifyOutputType>()
                       .AddProjectCheck<NoPreReleaseNuGetPackages>()
                       .AddProjectCheck<NuGetPolicyDisableImplicitNuGetFallbackFolder>()
                       .AddProjectCheck<OnlyExesShouldBePublishablePolicy>()
                       .AddProjectCheck<RunAotCompilationPolicy>()
                       .AddProjectCheck<TargetFrameworkIsSetCorrectlyPolicy>()
                       .AddProjectCheck<TieredCompilationPolicy>()
                       .AddProjectCheck<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy>()
                       .AddProjectCheck<JsonSerializerIsReflectionEnabledByDefaultPolicy>()
                       .AddProjectCheck<OptimizationPreferencePolicy>();
    }

    private static IServiceCollection PublishingSettings(this IServiceCollection services)
    {
        return services.AddProjectCheck<DebuggerSupportPolicy>()
                       .AddProjectCheck<IlcGenerateStackTraceDataPolicy>()
                       .AddProjectCheck<IlcOptimizationPreferencePolicy>()
                       .AddProjectCheck<PublishableExesMustHaveRuntimeIdentifiers>()
                       .AddProjectCheck<TieredPgoPolicy>()
                       .AddProjectCheck<UseSystemResourceKeysPolicy>()
                       .AddProjectCheck<ValidateExecutableReferencesMatchSelfContainedPolicy>();
    }

    private static IServiceCollection PackagingSettings(this IServiceCollection services)
    {
        return services.AddProjectCheck<DescriptionMetadata>()
                       .AddProjectCheck<EnablePackageValidationPolicy>()
                       .AddProjectCheck<IncludeSymbolsPolicy>()
                       .AddProjectCheck<SymbolPackageFormatPolicy>()
                       .AddProjectCheck<ImportCommonProps>()
                       .AddProjectCheck<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects>()
                       .AddProjectCheck<PackableLibrariesShouldNotDependOnNonPackable>()
                       .AddProjectCheck<PackageTagsMetadata>()
                       .AddProjectCheck<RepositoryUrlMetadata>();
    }

    [SuppressMessage(category: "Philips.CodeAnalysis.DuplicateCodeAnalyzer", checkId: "PH2071: Duplicate code shape", Justification = "Registering Analyzers")]
    private static IServiceCollection StaticCodeAnalysis(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        return services.AddProjectCheck<AnalysisLevelPolicyUseLatestVersion>()
                       .AddProjectCheck<AnalysisModePolicy>()
                       .AddProjectCheck<CodeAnalysisTreatWarningsAsErrorsPolicy>()
                       .AddProjectCheck<EnableNetAnalyzersPolicy>()
                       .AddProjectCheck<EnforceCodeStyleInBuildPolicy>()
                       .AddProjectCheck<EnforceExtendedAnalyzerRulesPolicy>()
                       .AddProjectCheck<ErrorPolicyWarningAsErrors>()
                       .AddProjectCheck<MustHaveAsyncAnalyzerPackage>()
                       .AddProjectCheck<MustHaveCodecrackerCSharpAnalyzerPackage>()
                       .AddProjectCheck<MustHaveMeziantouAnalyzerPackage>()
                       .AddProjectCheck<MustHaveDuplicateCodeAnalyzerPackage>()
                       .AddProjectCheck<MustHaveSecurityCodeScanAnalyzerPackage>()
                       .AddProjectCheck<MustHaveSmartAnalyzerPackage>()
                       .AddProjectCheck<MustHavePhilipsMaintainabilityAnalyzerPackage>()
                       .AddProjectCheck<MustEnableStrictMode>()
                       .AddProjectCheck<MustHaveRoslynatorAnalyzersPackage>()
                       .AddProjectCheck<MustHaveSonarAnalyzerPackage>()
                       .AddProjectCheck<MustHaveThreadingAnalyzerPackage>()
                       .AddProjectCheck<MustHaveToStringWithoutOverrideAnalyzerPackage>()
                       .AddProjectCheck<MustNotHaveFxCopAnalyzerPackage>()
                       .AddProjectCheck<MustUseOpenApiAnalyzers>()
                       .AddProjectCheck<NuGetAuditPolicy>()
                       .AddEnumSourceGeneratorAnalyzerPackage(repositorySettings: repositorySettings)
                       .AddUnitTestAnalysers(repositorySettings: repositorySettings)
                       .AddFunFairCodeAnalysisRequirements(repositorySettings: repositorySettings);
    }

    private static IServiceCollection AddFunFairCodeAnalysisRequirements(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        return repositorySettings.IsCodeAnalysisSolution
            ? services
            : services.AddProjectCheck<MustHaveFunFairCodeAnalysisPackage>();
    }

    private static IServiceCollection AddEnumSourceGeneratorAnalyzerPackage(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        return repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage
            ? services
            : services.AddProjectCheck<MustHaveEnumSourceGeneratorAnalyzerPackage>();
    }

    private static IServiceCollection AddUnitTestAnalysers(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        if (repositorySettings.IsUnitTestBase)
        {
            return services;
        }

        return services.AddProjectCheck<UsingNSubstituteMustIncludeAnalyzer>()
                       .AddProjectCheck<UsingXUnitMustIncludeVisualStudioTestPlatform>()
                       .AddProjectCheck<UsingXUnitMustIncludeTeamCityTestAdapter>()
                       .AddProjectCheck<UsingXUnitMustIncludeCoverletCollector>()
                       .AddProjectCheck<UsingXUnitMustIncludeCoverletMsBuild>()
                       .AddProjectCheck<UsingXUnitMustIncludeAnalyzer>();
    }

    private static IServiceCollection AddProjectCheck<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
        where T : class, IProjectCheck
    {
        Console.WriteLine($"* Project Check: {typeof(T).Name}");

        return services.AddSingleton<IProjectCheck, T>();
    }

    private static IServiceCollection AddSolutionCheck<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services)
        where T : class, ISolutionCheck
    {
        Console.WriteLine($"* Solution Check: {typeof(T).Name}");

        return services.AddSingleton<ISolutionCheck, T>();
    }
}