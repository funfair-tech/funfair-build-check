using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Coverlet Ms Build is installed for test projects.
/// </summary>
public sealed class UsingXUnitMustIncludeCoverletMsBuild : HasAppropriateNonAnalysisPackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingXUnitMustIncludeCoverletMsBuild(ILogger<UsingXUnitMustIncludeCoverletMsBuild> logger)
        : base(detectPackageId: @"xunit", mustIncludePackageId: @"coverlet.msbuild", logger: logger)
    {
    }
}