using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class UseSystemResourceKeysPolicy : SimplePropertyProjectCheckBase
{
    public UseSystemResourceKeysPolicy(ILogger<UseSystemResourceKeysPolicy> logger)
        : base(propertyName: "UseSystemResourceKeys", requiredValue: "true", logger: logger) { }
}
