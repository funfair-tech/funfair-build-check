using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveDuplicateCodeAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveDuplicateCodeAnalyzerPackage(ILogger<MustHaveDuplicateCodeAnalyzerPackage> logger)
        : base(packageId: "Philips.CodeAnalysis.DuplicateCodeAnalyzer", mustHave: true, logger: logger)
    {
    }
}