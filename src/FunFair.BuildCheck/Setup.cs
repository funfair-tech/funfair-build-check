using System;
using System.Diagnostics.CodeAnalysis;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.References;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.SolutionChecks;
using Microsoft.Extensions.DependencyInjection;

namespace FunFair.BuildCheck;

internal static class Setup
{
    public static IServiceCollection SetupSolutionChecks(this IServiceCollection services)
    {
        return services.AddSolutionCheck<GlobalJsonIsLatest>()
                       .AddSolutionCheck<GlobalJsonMustSpecifyCorrectRollforwardPolicy>()
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
        return StringComparer.InvariantCulture.Equals(Environment.GetEnvironmentVariable("XML_DOCUMENTATION"), y: "true")
            ? services.AddProjectCheck<XmlDocumentationFileRequiredPolicy>()
            : services.AddProjectCheck<XmlDocumentationFileProhibitedPolicy>();
    }

    [SuppressMessage(category: "Philips.CodeAnalysis.DuplicateCodeAnalyzer", checkId: "PH2071: Duplicate code shape", Justification = "Registering Analyzers")]
    private static IServiceCollection GeneralProjectSettings(this IServiceCollection services)
    {
        services = services.AddProjectCheck<DoesNotUseDotNetCliToolReference>()
                           .AddProjectCheck<DoesNotUseRootNamespace>()
                           .AddProjectCheck<GenerateNeutralResourcesLanguageAttributePolicy>()
                           .AddProjectCheck<ImplicitUsingsPolicy>()
                           .AddProjectCheck<LanguagePolicyUseLatestVersion>()
                           .AddProjectCheck<LibrariesShouldBePackablePolicy>()
                           .AddProjectCheck<MustEnableNullable>()
                           .AddProjectCheck<MustNotDisableUnexpectedWarnings>()
                           .AddProjectCheck<MustSpecifyOutputType>()
                           .AddProjectCheck<NoPreReleaseNuGetPackages>()
                           .AddProjectCheck<NuGetPolicyDisableImplicitNuGetFallbackFolder>();

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_PUBLISHABLE")))
        {
            services = services.AddProjectCheck<OnlyExesShouldBePublishablePolicy>();
        }

        services = services.AddProjectCheck<RunAotCompilationPolicy>();

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_CORE_APP_TARGET_FRAMEWORK")))
        {
            services = services.AddProjectCheck<TargetFrameworkIsSetCorrectlyPolicy>();
        }

        return services.AddProjectCheck<TieredCompilationPolicy>();
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
        services = services.AddProjectCheck<DescriptionMetadata>()
                           .AddProjectCheck<EnablePackageValidationPolicy>();

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_PACK_PROJECT_METADATA_IMPORT")))
        {
            services = services.AddProjectCheck<ImportCommonProps>();
        }

        return services.AddProjectCheck<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects>()
                       .AddProjectCheck<PackableLibrariesShouldNotDependOnNonPackable>()
                       .AddProjectCheck<PackageTagsMetadata>()
                       .AddProjectCheck<RepositoryUrlMetadata>();
    }

    [SuppressMessage(category: "Philips.CodeAnalysis.DuplicateCodeAnalyzer", checkId: "PH2071: Duplicate code shape", Justification = "Registering Analyzers")]
    private static IServiceCollection StaticCodeAnalysis(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("EXCLUDE_DOTNET_SOURCE_GENERATION")))
        {
            services = services.AddProjectCheck<MustHaveEnumSourceGeneratorAnalyzerPackage>();
        }

        return services.AddProjectCheck<AnalysisLevelPolicyUseLatestVersion>()
                       .AddProjectCheck<AnalysisModePolicy>()
                       .AddProjectCheck<CodeAnalysisTreatWarningsAsErrorsPolicy>()
                       .AddProjectCheck<EnableNetAnalyzersPolicy>()
                       .AddProjectCheck<EnforceCodeStyleInBuildPolicy>()
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
                       .AddUnitTestAnalysers(repositorySettings: repositorySettings)
                       .AddFunFairCodeAnalysisRequirements(repositorySettings: repositorySettings);
    }

    private static IServiceCollection AddFunFairCodeAnalysisRequirements(this IServiceCollection services, IRepositorySettings repositorySettings)
    {
        return repositorySettings.IsCodeAnalysisSolution
            ? services
            : services.AddProjectCheck<MustHaveFunFairCodeAnalysisPackage>();
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