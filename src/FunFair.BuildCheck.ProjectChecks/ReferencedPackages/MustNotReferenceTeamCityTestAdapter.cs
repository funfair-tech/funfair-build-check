using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotReferenceTeamCityTestAdapter : MustNotReferencePackages
{
    public MustNotReferenceTeamCityTestAdapter(ILogger<MustNotReferenceTeamCityTestAdapter> logger)
        : base(["TeamCity.VSTest.TestAdapter"], reason: "No longer using teamcity", logger: logger)
    { }
}
