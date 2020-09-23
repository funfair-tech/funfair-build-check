using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
    {
        public MustHaveSonarAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"SonarAnalyzer.CSharp", logger: logger)
        {
        }
    }
}