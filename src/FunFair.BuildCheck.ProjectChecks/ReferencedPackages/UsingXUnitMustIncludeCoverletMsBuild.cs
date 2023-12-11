using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitMustIncludeCoverletMsBuild : HasAppropriateNonAnalysisPackages
{
    public UsingXUnitMustIncludeCoverletMsBuild(ILogger<UsingXUnitMustIncludeCoverletMsBuild> logger)
        : base(detectPackageId: "xunit", mustIncludePackageId: "coverlet.msbuild", logger: logger)
    {
    }
}