using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveFunFairCodeAnalysisPackage : MustHaveAnalyzerPackage
{
    public MustHaveFunFairCodeAnalysisPackage(ILogger<MustHaveFunFairCodeAnalysisPackage> logger)
        : base(packageId: @"FunFair.CodeAnalysis", mustHave: true, logger: logger)
    {
    }
}