using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveToStringWithoutOverrideAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveToStringWithoutOverrideAnalyzerPackage(ILogger<MustHaveToStringWithoutOverrideAnalyzerPackage> logger)
        : base(packageId: @"ToStringWithoutOverrideAnalyzer", mustHave: true, logger: logger)
    {
    }
}