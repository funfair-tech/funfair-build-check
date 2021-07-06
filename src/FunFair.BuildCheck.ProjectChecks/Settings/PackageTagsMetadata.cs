using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    /// Checks that the &quot;PackageTags&quot; property is set in the project.
    /// </summary>
    public sealed class PackageTagsMetadata : PackableMetadataBase
    {
        public PackageTagsMetadata(ILogger<PackageTagsMetadata> logger)
            : base(@"PackageTags", logger)
        {
        }
    }
}