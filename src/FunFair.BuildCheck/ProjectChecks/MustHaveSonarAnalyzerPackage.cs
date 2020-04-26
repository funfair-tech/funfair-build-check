using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveSonarAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"SonarAnalyzer.CSharp", logger)
        {
        }
    }
}