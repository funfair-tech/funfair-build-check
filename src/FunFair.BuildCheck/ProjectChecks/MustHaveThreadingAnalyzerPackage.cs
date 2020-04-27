using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class MustHaveThreadingAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveThreadingAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"Microsoft.VisualStudio.Threading.Analyzers", logger)
        {
        }
    }
}