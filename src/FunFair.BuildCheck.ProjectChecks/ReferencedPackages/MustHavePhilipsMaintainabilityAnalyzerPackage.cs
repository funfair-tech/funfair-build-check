using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Philips maintainability issues analyzer is installed.
/// </summary>
public sealed class MustHavePhilipsMaintainabilityAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHavePhilipsMaintainabilityAnalyzerPackage(ILogger<MustHavePhilipsMaintainabilityAnalyzerPackage> logger)
        : base(packageId: @"Philips.CodeAnalysis.MaintainabilityAnalyzers", mustHave: true, logger: logger)
    {
    }
}