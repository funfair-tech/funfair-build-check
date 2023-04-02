using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitMustIncludeTeamCityTestAdapter : HasAppropriateNonAnalysisPackages
{
    public UsingXUnitMustIncludeTeamCityTestAdapter(ILogger<UsingXUnitMustIncludeTeamCityTestAdapter> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"TeamCity.VSTest.TestAdapter", logger: logger)
    {
    }
}