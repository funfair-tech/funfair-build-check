using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MustHaveToStringWithoutOverrideAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveToStringWithoutOverrideAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"ToStringWithoutOverrideAnalyzer", logger: logger)
        {
        }
    }
}