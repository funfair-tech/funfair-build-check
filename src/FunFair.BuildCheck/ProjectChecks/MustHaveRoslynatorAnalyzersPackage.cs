using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MustHaveRoslynatorAnalyzersPackage : MustHaveAnalyzerPackage
    {
        public MustHaveRoslynatorAnalyzersPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
            : base(packageId: @"Roslynator.Analyzers", logger: logger)
        {
        }
    }
}