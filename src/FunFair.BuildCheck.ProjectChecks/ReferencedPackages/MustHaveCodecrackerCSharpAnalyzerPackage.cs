using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveCodecrackerCSharpAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveCodecrackerCSharpAnalyzerPackage(ILogger<MustHaveCodecrackerCSharpAnalyzerPackage> logger)
        : base(packageId: @"codecracker.CSharp", mustHave: true, logger: logger)
    {
    }
}