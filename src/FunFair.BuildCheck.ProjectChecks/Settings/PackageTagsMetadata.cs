using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PackageTagsMetadata : PackableMetadataBase
{
    public PackageTagsMetadata(ILogger<PackageTagsMetadata> logger)
        : base(property: "PackageTags", logger: logger) { }
}
