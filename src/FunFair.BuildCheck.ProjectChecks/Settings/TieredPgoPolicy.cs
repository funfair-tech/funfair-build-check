using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TieredPgoPolicy : SimplePropertyProjectCheckBase
{
    public TieredPgoPolicy(ILogger<TieredPgoPolicy> logger)
        : base(propertyName: "TieredPGO", requiredValue: "true", logger: logger) { }
}
