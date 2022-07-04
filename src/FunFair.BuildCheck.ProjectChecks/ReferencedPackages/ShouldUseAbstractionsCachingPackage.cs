using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the caching abstractions package is used rather than the full version.
/// </summary>
public sealed class ShouldUseAbstractionsCachingPackage : ShouldUseAlternatePackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ShouldUseAbstractionsCachingPackage(ILogger<ShouldUseAbstractionsCachingPackage> logger)
        : base(matchPackageId: @"Microsoft.Extensions.Caching.Memory", usePackageId: @"Microsoft.Extensions.Caching.Abstractions", logger: logger)
    {
    }
}