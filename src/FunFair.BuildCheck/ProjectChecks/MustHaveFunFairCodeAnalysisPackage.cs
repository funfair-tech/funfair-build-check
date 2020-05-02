using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class MustHaveFunFairCodeAnalysisPackage : MustHaveAnalyzerPackage
    {
        public MustHaveFunFairCodeAnalysisPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"FunFair.CodeAnalysis", logger: logger)
        {
        }
    }
}