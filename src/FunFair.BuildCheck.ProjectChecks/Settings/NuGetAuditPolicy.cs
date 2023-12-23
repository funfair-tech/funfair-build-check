using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NuGetAuditPolicy : SimplePropertyProjectCheckBase
{
    public NuGetAuditPolicy(ILogger<NuGetAuditPolicy> logger)
        : base(propertyName: "NuGetAudit", requiredValue: "true", logger: logger)
    {
    }
}