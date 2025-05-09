using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ValidateExecutableReferencesMatchSelfContainedPolicy : SimplePropertyProjectCheckBase
{
    public ValidateExecutableReferencesMatchSelfContainedPolicy(
        ILogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger
    )
        : base(propertyName: "ValidateExecutableReferencesMatchSelfContained", requiredValue: "true", logger: logger)
    { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return project.IsPublishable();
    }
}
