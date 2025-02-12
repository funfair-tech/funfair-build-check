using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnableNetAnalyzersPolicy : SimplePropertyProjectCheckBase
{
    public EnableNetAnalyzersPolicy(ILogger<EnableNetAnalyzersPolicy> logger)
        : base(propertyName: "EnableNETAnalyzers", requiredValue: "true", logger: logger) { }
}
