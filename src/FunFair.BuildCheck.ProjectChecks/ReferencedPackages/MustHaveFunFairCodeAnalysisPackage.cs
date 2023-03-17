using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the funfair code analyzer is installed.
/// </summary>
public sealed class MustHaveFunFairCodeAnalysisPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveFunFairCodeAnalysisPackage(ILogger<MustHaveFunFairCodeAnalysisPackage> logger)
        : base(packageId: @"FunFair.CodeAnalysis", mustHave: true, logger: logger)
    {
    }
}