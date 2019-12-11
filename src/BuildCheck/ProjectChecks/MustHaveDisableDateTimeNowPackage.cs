using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class MustHaveDisableDateTimeNowPackage : MustHaveAnalyzerPackage
    {
        public MustHaveDisableDateTimeNowPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"DisableDateTimeNow", logger)
        {
        }
    }
}