using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the xunit analyzer is installed for test projects.
/// </summary>
public sealed class UsingXUnitMustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingXUnitMustIncludeAnalyzer(ILogger<UsingXUnitMustIncludeAnalyzer> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"xunit.analyzers", logger: logger)
    {
    }
}