using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotHaveFxCopAnalyzerPackage : MustNotReferencePackages
{
    private static readonly string[] PackageIds =
    {
        "Microsoft.CodeAnalysis.FxCopAnalyzers",
        "Microsoft.CodeAnalysis.VersionCheckAnalyzer"
    };

    public MustNotHaveFxCopAnalyzerPackage(ILogger<MustNotHaveFxCopAnalyzerPackage> logger)
        : base(packageIds: PackageIds, reason: ".net core 5 and later include this by default", logger: logger)
    {
    }
}