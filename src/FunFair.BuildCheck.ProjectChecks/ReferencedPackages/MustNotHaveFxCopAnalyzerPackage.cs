using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that FX-Cop is not referenced for .net core 5.0 targets.
/// </summary>
public sealed class MustNotHaveFxCopAnalyzerPackage : MustNotReferencePackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
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