using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class DebuggerSupportPolicy : SimplePropertyProjectCheckBase
{
    public DebuggerSupportPolicy(ILogger<DebuggerSupportPolicy> logger)
        : base(propertyName: "DebuggerSupport", requiredValue: "true", logger: logger) { }
}
