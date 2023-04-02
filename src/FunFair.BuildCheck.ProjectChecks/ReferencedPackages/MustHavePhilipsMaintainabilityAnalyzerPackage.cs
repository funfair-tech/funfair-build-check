using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHavePhilipsMaintainabilityAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHavePhilipsMaintainabilityAnalyzerPackage(ILogger<MustHavePhilipsMaintainabilityAnalyzerPackage> logger)
        : base(packageId: @"Philips.CodeAnalysis.MaintainabilityAnalyzers", mustHave: true, logger: logger)
    {
    }
}