using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveMeziantouAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveMeziantouAnalyzerPackage(ILogger<MustHaveMeziantouAnalyzerPackage> logger)
        : base(packageId: "Meziantou.Analyzer", mustHave: true, logger: logger) { }
}
