using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitMustIncludeCoverletCollector : HasAppropriateNonAnalysisPackages
{
    public UsingXUnitMustIncludeCoverletCollector(ILogger<UsingXUnitMustIncludeCoverletCollector> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"coverlet.collector", logger: logger)
    {
    }
}