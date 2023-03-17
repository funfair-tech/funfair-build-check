using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Meziantou issues analyzer is installed.
/// </summary>
public sealed class MustHaveMeziantouAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveMeziantouAnalyzerPackage(ILogger<MustHaveMeziantouAnalyzerPackage> logger)
        : base(packageId: @"Meziantou.Analyzer", mustHave: true, logger: logger)
    {
    }
}