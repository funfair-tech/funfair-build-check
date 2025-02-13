using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitMustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    public UsingXUnitMustIncludeAnalyzer(ILogger<UsingXUnitMustIncludeAnalyzer> logger)
        : base(detectPackageId: "xunit", mustIncludePackageId: "xunit.analyzers", logger: logger)
    { }
}
