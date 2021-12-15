using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that FX-Cop is not referenced for .net core 5.0 targets.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class MustNotHaveFxCopAnalyzerPackage : MustNotReferencePackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustNotHaveFxCopAnalyzerPackage(ILogger<MustNotHaveFxCopAnalyzerPackage> logger)
        : base(new[]
               {
                   "Microsoft.CodeAnalysis.FxCopAnalyzers"
               },
               reason: ".net core 5 and later include this by default",
               logger: logger)
    {
    }
}