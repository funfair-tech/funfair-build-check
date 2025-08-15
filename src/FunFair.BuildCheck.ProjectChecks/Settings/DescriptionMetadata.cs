using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class DescriptionMetadata : PackableMetadataBase
{
    public DescriptionMetadata(ILogger<DescriptionMetadata> logger)
        : base(property: "Description", logger: logger) { }
}