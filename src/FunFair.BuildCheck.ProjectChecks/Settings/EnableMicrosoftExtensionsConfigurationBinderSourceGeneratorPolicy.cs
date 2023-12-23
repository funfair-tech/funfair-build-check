using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy : SimplePropertyProjectCheckBase
{
    public EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy(ILogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger)
        : base(propertyName: "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator", requiredValue: "true", logger: logger)
    {
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPackable();
    }
}