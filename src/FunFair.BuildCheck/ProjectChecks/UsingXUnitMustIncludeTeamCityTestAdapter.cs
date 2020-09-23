using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class UsingXUnitMustIncludeTeamCityTestAdapter : HasAppropriateNonAnalysisPackages
    {
        public UsingXUnitMustIncludeTeamCityTestAdapter(ILogger<NoPreReleaseNuGetPackages> logger)
            : base(detectPackageId: @"xunit", mustIncludePackageId: @"TeamCity.VSTest.TestAdapter", logger: logger)
        {
        }
    }
}