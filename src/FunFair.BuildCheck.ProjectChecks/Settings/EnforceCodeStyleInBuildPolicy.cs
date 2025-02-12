using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnforceCodeStyleInBuildPolicy : SimplePropertyProjectCheckBase
{
    public EnforceCodeStyleInBuildPolicy(ILogger<EnforceCodeStyleInBuildPolicy> logger)
        : base(propertyName: "EnforceCodeStyleInBuild", requiredValue: "true", logger: logger) { }
}
