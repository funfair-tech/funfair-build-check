using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the file providers abstractions package is used rather than the full version.
/// </summary>
public sealed class ShouldUseAbstractionsFileProvidersPackage : ShouldUseAlternatePackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ShouldUseAbstractionsFileProvidersPackage(ILogger<ShouldUseAbstractionsFileProvidersPackage> logger)
        : base(matchPackageId: @"Microsoft.Extensions.FileProviders", usePackageId: @"Microsoft.Extensions.FileProviders.Abstractions", logger: logger)
    {
    }
}