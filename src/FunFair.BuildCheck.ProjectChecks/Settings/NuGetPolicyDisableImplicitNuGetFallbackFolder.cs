using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NuGetPolicyDisableImplicitNuGetFallbackFolder : SimplePropertyProjectCheckBase
{
    public NuGetPolicyDisableImplicitNuGetFallbackFolder(ILogger<NuGetPolicyDisableImplicitNuGetFallbackFolder> logger)
        : base(propertyName: "DisableImplicitNuGetFallbackFolder", requiredValue: "true", logger: logger)
    {
    }
}