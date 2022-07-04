using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Coverlet Collector is installed for test projects.
/// </summary>
public sealed class UsingXUnitMustIncludeCoverletCollector : HasAppropriateNonAnalysisPackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingXUnitMustIncludeCoverletCollector(ILogger<UsingXUnitMustIncludeCoverletCollector> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"coverlet.collector", logger: logger)
    {
    }
}