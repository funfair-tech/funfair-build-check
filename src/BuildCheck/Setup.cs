using BuildCheck.ProjectChecks;
using Microsoft.Extensions.DependencyInjection;

namespace BuildCheck
{
    internal static class Setup
    {
        public static void SetupSolutionChecks(IServiceCollection services)
        {

        }

        public static void SetupProjectChecks(IServiceCollection services)
        {
            services.AddSingleton<IProjectCheck, NoPreReleaseNuGetPackages>();
            services.AddSingleton<IProjectCheck, ErrorPolicyWarningAsErrors>();
            services.AddSingleton<IProjectCheck, LanguagePolicyUseLatestVersion>();
            services.AddSingleton<IProjectCheck, NuGetPolicyDisableImplicitNuGetFallbackFolder>();
        }
    }
}