using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class AnalysisLevelPolicyUseLatestVersion : SimplePropertyProjectCheckBase
{
    public AnalysisLevelPolicyUseLatestVersion(ILogger<AnalysisLevelPolicyUseLatestVersion> logger)
        : base(propertyName: "AnalysisLevel", requiredValue: "latest", logger: logger) { }
}
