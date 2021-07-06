using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    /// Checks that the &quot;Description&quot; property is set in the project.
    /// </summary>
    public sealed class DescriptionMetadata : PackableMetadataBase
    {
        public DescriptionMetadata(ILogger<DescriptionMetadata> logger)
            : base(@"Description", logger)
        {
        }
    }
}