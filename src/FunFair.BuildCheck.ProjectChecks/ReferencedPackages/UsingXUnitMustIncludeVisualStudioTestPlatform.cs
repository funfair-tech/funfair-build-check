using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingXUnitMustIncludeVisualStudioTestPlatform : HasAppropriateNonAnalysisPackages
{
    public UsingXUnitMustIncludeVisualStudioTestPlatform(ILogger<UsingXUnitMustIncludeVisualStudioTestPlatform> logger)
        : base(detectPackageId: "xunit", mustIncludePackageId: "Microsoft.NET.Test.Sdk", logger: logger) { }
}
