using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class RunAotCompilationPolicy : SimplePropertyProjectCheckBase
{
    public RunAotCompilationPolicy(ILogger<RunAotCompilationPolicy> logger)
        : base(propertyName: "RunAOTCompilation", requiredValue: "false", logger: logger) { }
}
