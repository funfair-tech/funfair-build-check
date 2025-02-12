using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveSonarAnalyzerPackage(ILogger<MustHaveSonarAnalyzerPackage> logger)
        : base(packageId: "SonarAnalyzer.CSharp", mustHave: true, logger: logger) { }
}
