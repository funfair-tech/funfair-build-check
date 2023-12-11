using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveSmartAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveSmartAnalyzerPackage(ILogger<MustHaveSmartAnalyzerPackage> logger)
        : base(packageId: "SmartAnalyzers.CSharpExtensions.Annotations", mustHave: true, logger: logger)
    {
    }
}