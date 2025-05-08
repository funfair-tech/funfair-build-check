using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy : SimplePropertyProjectCheckBase
{
    public PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy(
        ILogger<PublishableProjectsShouldEnableRequestDelegateGeneratorPolicy> logger
    )
        : base(propertyName: "EnableRequestDelegateGenerator", requiredValue: "true", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(project.GetOutputType(), y: "Exe") && project.IsPublishable();
    }
}
