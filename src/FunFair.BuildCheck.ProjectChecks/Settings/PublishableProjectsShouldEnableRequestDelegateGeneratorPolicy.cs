using System;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy : SimplePropertyProjectCheckBase
{
    public PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy(ILogger<PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy> logger)
        : base(propertyName: "EnableRequestDelegateGenerator", requiredValue: "true", logger: logger)
    {
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return StringComparer.InvariantCultureIgnoreCase.Equals(project.GetOutputType(), y: "Exe") && project.IsPublishable();
    }
}