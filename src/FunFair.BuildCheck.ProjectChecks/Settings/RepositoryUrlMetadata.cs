using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that the &quot;RepositoryUrl&quot; property is set in the project.
    /// </summary>
    public sealed class RepositoryUrlMetadata : PackableMetadataBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public RepositoryUrlMetadata(ILogger<RepositoryUrlMetadata> logger)
            : base(property: @"RepositoryUrl", logger: logger)
        {
        }
    }
}