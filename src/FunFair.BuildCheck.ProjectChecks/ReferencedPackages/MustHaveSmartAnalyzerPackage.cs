using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Smart issues analyzer is installed.
/// </summary>
public sealed class MustHaveSmartAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSmartAnalyzerPackage(ILogger<MustHaveSmartAnalyzerPackage> logger)
        : base(packageId: @"SmartAnalyzers.CSharpExtensions.Annotations", logger: logger)
    {
    }
}