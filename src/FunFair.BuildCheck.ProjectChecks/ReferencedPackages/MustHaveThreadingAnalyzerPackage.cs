using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveThreadingAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveThreadingAnalyzerPackage(ILogger<MustHaveThreadingAnalyzerPackage> logger)
        : base(
            packageId: "Microsoft.VisualStudio.Threading.Analyzers",
            mustHave: true,
            logger: logger
        ) { }
}
