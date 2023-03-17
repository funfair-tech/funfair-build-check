using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the ToString misuse analyzer is installed.
/// </summary>
public sealed class MustHaveToStringWithoutOverrideAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveToStringWithoutOverrideAnalyzerPackage(ILogger<MustHaveToStringWithoutOverrideAnalyzerPackage> logger)
        : base(packageId: @"ToStringWithoutOverrideAnalyzer", mustHave: true, logger: logger)
    {
    }
}