using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy
    : SimplePropertyProjectCheckBase
{
    public PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy(
        ILogger<PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy> logger
    )
        : base(propertyName: "PublishTrimmed", requiredValue: "true", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
#if PUBLISH_TRIMMED_BUILDS_CLEAN
        if (!project.IsPublishable())
        {
            return false;
        }

        string? value = project.GetProperty("EnableTrimAnalyzer");

        return !string.IsNullOrWhiteSpace(value) && StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "true");
#endif
        return false;
    }
}
