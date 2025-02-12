using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveAsyncAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveAsyncAnalyzerPackage(ILogger<MustHaveAsyncAnalyzerPackage> logger)
        : base(packageId: "AsyncFixer", mustHave: true, logger: logger) { }
}
