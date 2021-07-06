using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    /// Checks that the &quot;RepositoryUrl&quot; property is set in the project.
    /// </summary>
    public sealed class RepositoryUrlMetadata : PackableMetadataBase
    {
        public RepositoryUrlMetadata(ILogger<RepositoryUrlMetadata> logger)
            : base(@"RepositoryUrl", logger)
        {
        }
    }
}