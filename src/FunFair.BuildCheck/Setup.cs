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
            AddSolutionCheck<GlobalJsonIsLatest>(services);
            AddSolutionCheck<GlobalJsonMustSpecifyRollforwardDisable>(services);
            AddSolutionCheck<GlobalJsonMustNotAllowPreRelease>(services);
        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
            AddProjectCheck<MustSpecifyOutputType>(services);
            AddProjectCheck<NoPreReleaseNuGetPackages>(services);
            AddProjectCheck<ErrorPolicyWarningAsErrors>(services);
            AddProjectCheck<LanguagePolicyUseLatestVersion>(services);
            AddProjectCheck<NuGetPolicyDisableImplicitNuGetFallbackFolder>(services);
            AddProjectCheck<DoesNotReferenceByDll>(services);
            AddProjectCheck<MustHaveSourceLinkPackage>(services);
            AddProjectCheck<MustHaveFxCopAnalyzerPackage>(services);
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

            if (!IsUnitTestBase())
            {
                AddProjectCheck<UsingXUnitMustIncludeVisualStudioTestPlatform>(services);
                AddProjectCheck<UsingXUnitMustIncludeTeamCityTestAdapter>(services);
            }

            if (!IsCodeAnalysisSolution())
            {
                AddProjectCheck<MustHaveFunFairCodeAnalysisPackage>(services);
            }

            AddProjectCheck<ShouldUseAbstractionsDependencyInjectionPackage>(services);

#if DOTNET_VERSION_SPECIFIC_TESTS
            string? dotnetVersion = Environment.GetEnvironmentVariable(variable: @"DOTNET_CORE_SDK_VERSION");

            if (!string.IsNullOrWhiteSpace(dotnetVersion) && new Version(dotnetVersion) > new Version(major: 3, minor: 1, build: 101))
            {
                AddProjectCheck<MustHaveThreadingAnalyzerPackage>(services);
            }
#endif

            if (IsNullableGloballyEnforced())
            {
                AddProjectCheck<MustEnableNullable>(services);
            }
        }

        private static bool IsCodeAnalysisSolution()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"BUILD_CODEANALYSIS");

            return !string.IsNullOrWhiteSpace(codeAnalysis) && StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }

        private static bool IsNullableGloballyEnforced()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"DISABLE_BUILD_NULLABLE_REFERENCE_TYPES");

            return string.IsNullOrWhiteSpace(codeAnalysis) || !StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }

        private static bool IsUnitTestBase()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"IS_UNITTEST_BASE");

            return !string.IsNullOrWhiteSpace(codeAnalysis) || StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
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