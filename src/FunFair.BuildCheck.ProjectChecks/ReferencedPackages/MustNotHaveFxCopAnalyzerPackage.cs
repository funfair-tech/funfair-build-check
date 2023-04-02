using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotHaveFxCopAnalyzerPackage : MustNotReferencePackages
{
    public MustNotHaveFxCopAnalyzerPackage(ILogger<MustNotHaveFxCopAnalyzerPackage> logger)
        : base(new[]
               {
                   "Microsoft.CodeAnalysis.FxCopAnalyzers",
                   "Microsoft.CodeAnalysis.VersionCheckAnalyzer"
               },
               reason: ".net core 5 and later include this by default",
               logger: logger)
    {
    }
}