using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy : SimplePropertyProjectCheckBase
{
    public EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy(
        ILogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger
    )
        : base(
            propertyName: "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
            requiredValue: "true",
            logger: logger
        ) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return project.IsPackable();
    }
}
