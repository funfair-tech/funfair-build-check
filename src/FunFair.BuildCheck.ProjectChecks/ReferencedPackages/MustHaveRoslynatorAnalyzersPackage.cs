using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveRoslynatorAnalyzersPackage : MustHaveAnalyzerPackage
{
    public MustHaveRoslynatorAnalyzersPackage(ILogger<MustHaveRoslynatorAnalyzersPackage> logger)
        : base(packageId: "Roslynator.Analyzers", mustHave: true, logger: logger)
    {
    }
}