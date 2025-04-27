using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class AnalysisModePolicy : SimplePropertyProjectCheckBase
{
    public AnalysisModePolicy(ILogger<AnalysisModePolicy> logger)
        : base(propertyName: "AnalysisMode", requiredValue: "AllEnabledByDefault", logger: logger) { }
}
