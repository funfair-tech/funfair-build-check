using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class MustHaveRoslynatorAnalyzersPackage : MustHaveAnalyzerPackage
    {
        public MustHaveRoslynatorAnalyzersPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"Roslynator.Analyzers", logger)
        {
        }
    }
}