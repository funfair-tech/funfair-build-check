using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the async issues analyzer is installed.
/// </summary>
public sealed class MustHaveAsyncAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveAsyncAnalyzerPackage(ILogger<MustHaveAsyncAnalyzerPackage> logger)
        : base(packageId: @"AsyncFixer", logger: logger)
    {
    }
}