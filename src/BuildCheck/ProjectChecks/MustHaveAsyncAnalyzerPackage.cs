using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class MustHaveAsyncAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveAsyncAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"AsyncFixer", logger)
        {
        }
    }
}