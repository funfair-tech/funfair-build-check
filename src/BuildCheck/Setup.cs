using BuildCheck.ProjectChecks;
using BuildCheck.SolutionChecks;
using Microsoft.Extensions.DependencyInjection;

namespace BuildCheck
{
    internal static class Setup
    {
        public static void SetupSolutionChecks(IServiceCollection services)
        {
            services.AddSingleton<ISolutionCheck, AllProjectsExist>();
        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
            services.AddSingleton<IProjectCheck, NoPreReleaseNuGetPackages>();
            services.AddSingleton<IProjectCheck, ErrorPolicyWarningAsErrors>();
            services.AddSingleton<IProjectCheck, LanguagePolicyUseLatestVersion>();
            services.AddSingleton<IProjectCheck, NuGetPolicyDisableImplicitNuGetFallbackFolder>();
            services.AddSingleton<IProjectCheck, DotNetXUnitRunnerIsSameVersionAsPackage>();
            services.AddSingleton<IProjectCheck, DoesNotReferenceByDll>();
        }
    }
}