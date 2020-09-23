using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MustHaveAsyncAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveAsyncAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"AsyncFixer", logger: logger)
        {
        }
    }
}