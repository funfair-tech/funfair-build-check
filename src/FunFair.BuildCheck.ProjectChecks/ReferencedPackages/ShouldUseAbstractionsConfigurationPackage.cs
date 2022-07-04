using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the configuration abstractions package is used rather than the full version.
/// </summary>
public sealed class ShouldUseAbstractionsConfigurationPackage : ShouldUseAlternatePackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ShouldUseAbstractionsConfigurationPackage(ILogger<ShouldUseAbstractionsConfigurationPackage> logger)
        : base(matchPackageId: @"Microsoft.Extensions.Configuration", usePackageId: @"Microsoft.Extensions.Configuration.Abstractions", logger: logger)
    {
    }
}