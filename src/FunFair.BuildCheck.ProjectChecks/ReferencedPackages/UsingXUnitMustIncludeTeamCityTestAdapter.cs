using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the team city test adapter is installed for test projects.
/// </summary>
public sealed class UsingXUnitMustIncludeTeamCityTestAdapter : HasAppropriateNonAnalysisPackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingXUnitMustIncludeTeamCityTestAdapter(ILogger<UsingXUnitMustIncludeTeamCityTestAdapter> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"TeamCity.VSTest.TestAdapter", logger: logger)
    {
    }
}