using System;
using FunFair.BuildCheck.ProjectChecks;
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
            AddSolutionCheck<GlobalJsonMustSpecifyRollforwardDisable>(services);
            AddSolutionCheck<GlobalJsonMustNotAllowPreRelease>(services);
        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
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
            AddProjectCheck<DoesNotUseDotNetCliToolReference>(services);
            AddProjectCheck<LibrariesShouldNotDependOnExecutables>(services);

            if (!Classifications.IsUnitTestBase())
            {
                AddProjectCheck<UsingXUnitMustIncludeVisualStudioTestPlatform>(services);
                AddProjectCheck<UsingXUnitMustIncludeTeamCityTestAdapter>(services);
            }

            if (!Classifications.IsCodeAnalysisSolution())
            {
                AddProjectCheck<MustHaveFunFairCodeAnalysisPackage>(services);
            }

            AddProjectCheck<ShouldUseAbstractionsConfigurationPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsDependencyInjectionPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsFileProvidersPackage>(services);
            AddProjectCheck<ShouldUseAbstractionsLoggingPackage>(services);
            AddProjectCheck<ShouldUseFluentValidationAspNetCoreRatherThanFluentValidationPackage>(services);

            string? dotnetVersion = Environment.GetEnvironmentVariable(variable: @"DOTNET_CORE_SDK_VERSION");

            if (!string.IsNullOrWhiteSpace(dotnetVersion) && new Version(dotnetVersion) >= new Version(major: 5, minor: 0, build: 100))
            {
                AddProjectCheck<MustNotHaveFxCopAnalyzerPackage>(services);
            }
            else
            {
                AddProjectCheck<MustHaveFxCopAnalyzerPackage>(services);
            }

            if (Classifications.IsNullableGloballyEnforced())
            {
                AddProjectCheck<MustEnableNullable>(services);
            }
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