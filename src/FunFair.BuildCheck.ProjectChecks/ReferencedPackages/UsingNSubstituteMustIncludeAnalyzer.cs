using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingNSubstituteMustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    public UsingNSubstituteMustIncludeAnalyzer(ILogger<UsingNSubstituteMustIncludeAnalyzer> logger)
        : base(detectPackageId: "NSubstitute", mustIncludePackageId: "NSubstitute.Analyzers.CSharp", logger: logger)
    {
    }
}