using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveSonarAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"SonarAnalyzer.CSharp", logger)
        {
        }
    }
}