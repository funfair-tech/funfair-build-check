using System;
using BuildCheck.ProjectChecks;
using BuildCheck.SolutionChecks;
using Microsoft.Extensions.DependencyInjection;

namespace BuildCheck
{
    internal static class Setup
    {
        public static void SetupSolutionChecks(IServiceCollection services)
        {
            AddSolutionCheck<AllProjectsExist>(services);
            AddSolutionCheck<GlobalJsonIsLatest>(services);
        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
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

            string dotnetVersion = Environment.GetEnvironmentVariable(variable: @"DOTNET_CORE_SDK_VERSION");

            if (dotnetVersion != "2.2.401")
            {
                AddProjectCheck<MustUseOpenApiAnalyzers>(services);
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