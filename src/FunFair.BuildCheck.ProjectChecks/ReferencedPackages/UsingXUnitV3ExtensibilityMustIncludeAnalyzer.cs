using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitV3ExtensibilityMustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    public UsingXUnitV3ExtensibilityMustIncludeAnalyzer(
        ILogger<UsingXUnitV3ExtensibilityMustIncludeAnalyzer> logger
    )
        : base(
            detectPackageId: "xunit.v3.extensibility.core",
            mustIncludePackageId: "xunit.analyzers",
            logger: logger
        ) { }
}
