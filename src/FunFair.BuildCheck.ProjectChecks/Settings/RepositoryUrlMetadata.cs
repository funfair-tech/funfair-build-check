using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class RepositoryUrlMetadata : PackableMetadataBase
{
    public RepositoryUrlMetadata(ILogger<RepositoryUrlMetadata> logger)
        : base(property: "RepositoryUrl", logger: logger) { }
}
