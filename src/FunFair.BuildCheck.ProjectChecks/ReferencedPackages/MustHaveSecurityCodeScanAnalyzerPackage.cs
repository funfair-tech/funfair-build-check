using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveSecurityCodeScanAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveSecurityCodeScanAnalyzerPackage(
        ILogger<MustHaveSecurityCodeScanAnalyzerPackage> logger
    )
        : base(packageId: "SecurityCodeScan.VS2019", mustHave: true, logger: logger) { }
}
