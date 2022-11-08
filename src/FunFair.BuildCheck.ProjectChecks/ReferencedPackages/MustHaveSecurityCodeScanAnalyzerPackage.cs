using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Security Code Scan code issues analyzer is installed.
/// </summary>
public sealed class MustHaveSecurityCodeScanAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSecurityCodeScanAnalyzerPackage(ILogger<MustHaveSecurityCodeScanAnalyzerPackage> logger)
        : base(packageId: @"SecurityCodeScan.VS2019", logger: logger)
    {
    }
}