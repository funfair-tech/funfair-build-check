using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitV2MustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    public UsingXUnitV2MustIncludeAnalyzer(ILogger<UsingXUnitV2MustIncludeAnalyzer> logger)
        : base(detectPackageId: "xunit", mustIncludePackageId: "xunit.analyzers", logger: logger) { }
}
