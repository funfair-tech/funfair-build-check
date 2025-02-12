using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IlcOptimizationPreferencePolicy : SimplePropertyProjectCheckBase
{
    public IlcOptimizationPreferencePolicy(ILogger<IlcOptimizationPreferencePolicy> logger)
        : base(propertyName: "IlcOptimizationPreference", requiredValue: "Size", logger: logger) { }
}
