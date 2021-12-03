using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the funfair code analyzer is installed.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class MustHaveFunFairCodeAnalysisPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveFunFairCodeAnalysisPackage(ILogger<MustHaveFunFairCodeAnalysisPackage> logger)
        : base(packageId: @"FunFair.CodeAnalysis", logger: logger)
    {
    }
}