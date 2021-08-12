using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.References;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.SolutionChecks;
using Microsoft.Extensions.DependencyInjection;

namespace FunFair.BuildCheck
{
    internal static class Setup
    {
        public static void SetupSolutionChecks(IServiceCollection services)
        {
            AddSolutionCheck<AllProjectsExist>(services);
            AddSolutionCheck<NoOrphanedProjectsExist>(services);
            AddSolutionCheck<GlobalJsonIsLatest>(services);
            AddSolutionCheck<GlobalJsonMustSpecifyCorrectRollforwardPolicy>(services);
            AddSolutionCheck<GlobalJsonMustNotAllowPreRelease>(services);
        }

        public static void SetupProjectChecks(IRepositorySettings repositorySettings, IServiceCollection services)
        {
            // TODO: Remove Repository settings dependency here.
            AddProjectCheck<ReferencedProjectsMustExist>(services);
            AddProjectCheck<MustSpecifyOutputType>(services);
            AddProjectCheck<NoPreReleaseNuGetPackages>(services);
            AddProjectCheck<ErrorPolicyWarningAsErrors>(services);
            AddProjectCheck<LanguagePolicyUseLatestVersion>(services);
            AddProjectCheck<NuGetPolicyDisableImplicitNuGetFallbackFolder>(services);
            AddProjectCheck<DoesNotReferenceByDll>(services);
            AddProjectCheck<MustHaveSourceLinkPackage>(services);
            AddProjectCheck<HasConsistentNuGetPackages>(services);
            AddProjectCheck<MustEnableStrictMode>(services);
            AddProjectCheck<UsingNSubstituteMustIncludeAnalyzer>(services);
            AddProjectCheck<UsingXUnitMustIncludeAnalyzer>(services);
            AddProjectCheck<ReferencesNugetPackageOnlyOnce>(services);
            AddProjectCheck<MustHaveAsyncAnalyzerPackage>(services);
            AddProjectCheck<MustHaveSonarAnalyzerPackage>(services);
            AddProjectCheck<MustHaveRoslynatorAnalyzersPackage>(services);
            AddProjectCheck<MustNotDisableUnexpectedWarnings>(services);
            AddProjectCheck<MustHaveThreadingAnalyzerPackage>(services);
            AddProjectCheck<MustUseOpenApiAnalyzers>(services);
            AddProjectCheck<MustNotReferenceObsoleteAspNetPackages>(services);
            AddProjectCheck<MustHaveToStringWithoutOverrideAnalyzerPackage>(services);
            AddProjectCheck<AnalysisLevelPolicyUseLatestVersion>(services);
            AddProjectCheck<DocumentationFilePolicy>(services);
            AddProjectCheck<AnalysisModePolicy>(services);
            AddProjectCheck<EnableNetAnalyzersPolicy>(services);
            AddProjectCheck<CodeAnalysisTreatWarningsAsErrorsPolicy>(services);
            AddProjectCheck<EnforceCodeStyleInBuildPolicy>(services);
            AddProjectCheck<LibrariesShouldBePackablePolicy>(services);
            AddProjectCheck<OnlyExesShouldBePublishablePolicy>(services);
            AddProjectCheck<DoesNotUseDotNetCliToolReference>(services);
            AddProjectCheck<LibrariesShouldNotDependOnExecutables>(services);
            AddProjectCheck<DoesNotUseRootNamespace>(services);

            if (!repositorySettings.IsUnitTestBase)
            {
                AddProjectCheck<UsingXUnitMustIncludeVisualStudioTestPlatform>(services);
                AddProjectCheck<UsingXUnitMustIncludeTeamCityTestAdapter>(services);
                AddProjectCheck<UsingXUnitMustIncludeCoverletCollector>(services);
                AddProjectCheck<UsingXUnitMustIncludeCoverletMsBuild>(services);
            }

            if (!repositorySettings.IsCodeAnalysisSolution)
            {
                AddProjectCheck<MustHaveFunFairCodeAnalysisPackage>(services);
            }

            AddProjectCheck<ShouldUseAbstractionsConfigurationPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsDependencyInjectionPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsFileProvidersPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsLoggingPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsExtensionHostingPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsCachingPackage>(services);
            AddProjectCheck<ShouldUseFluentValidationAspNetCoreRatherThanFluentValidationPackage>(services);
            AddProjectCheck<ShouldNotReferenceAllMetaPackagesInPackableProjects>(services);

            bool earlierThanDotNet5 = IsEarlierThanDotNet5();

            if (earlierThanDotNet5)
            {
                AddProjectCheck<MustHaveFxCopAnalyzerPackage>(services);
            }
            else
            {
                AddProjectCheck<MustNotHaveFxCopAnalyzerPackage>(services);
            }

            AddProjectCheck<MustEnableNullable>(services);

            AddProjectCheck<PackageTagsMetadata>(services);
            AddProjectCheck<DescriptionMetadata>(services);
            AddProjectCheck<RepositoryUrlMetadata>(services);

            AddProjectCheck<ImportCommonProps>(services);
            AddProjectCheck<PublishableExesMustHaveRuntimeIdentifiers>(services);
        }

        private static bool IsEarlierThanDotNet5()
        {
            string? dotnetVersion = Environment.GetEnvironmentVariable(variable: @"DOTNET_CORE_SDK_VERSION");

            if (string.IsNullOrWhiteSpace(dotnetVersion))
            {
                Console.WriteLine("No .NET version configured");

                return false;
            }

            Console.WriteLine($"Expected .NET Version: {dotnetVersion}");

            Version version = new(dotnetVersion);

            return version < new Version(major: 5, minor: 0, build: 0);
        }

        private static void AddProjectCheck<T>(IServiceCollection services)
            where T : class, IProjectCheck
        {
            Console.WriteLine($"* Project Check: {typeof(T).Name}");
            services.AddSingleton<IProjectCheck, T>();
        }

        private static void AddSolutionCheck<T>(IServiceCollection services)
            where T : class, ISolutionCheck
        {
            Console.WriteLine($"* Solution Check: {typeof(T).Name}");
            services.AddSingleton<ISolutionCheck, T>();
        }
    }
}