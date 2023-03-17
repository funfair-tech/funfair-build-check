using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the threading issues analyzer is installed.
/// </summary>
public sealed class MustHaveThreadingAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveThreadingAnalyzerPackage(ILogger<MustHaveThreadingAnalyzerPackage> logger)
        : base(packageId: @"Microsoft.VisualStudio.Threading.Analyzers", mustHave: true, logger: logger)
    {
    }
}