using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ImplicitUsingsPolicy : SimplePropertyProjectCheckBase
{
    public ImplicitUsingsPolicy(ILogger<ImplicitUsingsPolicy> logger)
        : base(propertyName: "ImplicitUsings", requiredValue: "disable", logger: logger)
    {
    }
}