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
        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
            AddProjectCheck<NoPreReleaseNuGetPackages>(services);
            AddProjectCheck<ErrorPolicyWarningAsErrors>(services);
            AddProjectCheck<LanguagePolicyUseLatestVersion>(services);
            AddProjectCheck<NuGetPolicyDisableImplicitNuGetFallbackFolder>(services);
            AddProjectCheck<DotNetXUnitRunnerIsSameVersionAsPackage>(services);
            AddProjectCheck<DoesNotReferenceByDll>(services);
            AddProjectCheck<MustHaveSourceLinkPackage>(services);
            AddProjectCheck<MustHaveFxCopAnalyzerPackate>(services);
        }

        private static void AddProjectCheck<T>(IServiceCollection services)
            where T : class, IProjectCheck
        {
            services.AddSingleton<IProjectCheck, T>();
        }

        private static void AddSolutionCheck<T>(IServiceCollection services)
            where T : class, ISolutionCheck
        {
            services.AddSingleton<ISolutionCheck, T>();
        }
    }
}