using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the &quot;PackageTags&quot; property is set in the project.
/// </summary>
public sealed class PackageTagsMetadata : PackableMetadataBase
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public PackageTagsMetadata(ILogger<PackageTagsMetadata> logger)
        : base(property: @"PackageTags", logger: logger)
    {
    }
}