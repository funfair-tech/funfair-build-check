using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitV3MustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    public UsingXUnitV3MustIncludeAnalyzer(ILogger<UsingXUnitV3MustIncludeAnalyzer> logger)
        : base(detectPackageId: "xunit.v3", mustIncludePackageId: "xunit.analyzers", logger: logger)
    { }
}
