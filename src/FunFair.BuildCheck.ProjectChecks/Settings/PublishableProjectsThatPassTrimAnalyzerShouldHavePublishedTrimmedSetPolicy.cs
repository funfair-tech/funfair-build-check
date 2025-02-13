using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy
    : SimplePropertyProjectCheckBase
{
    public PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy(
        ILogger<PublishableProjectsThatPassTrimAnalyzerShouldHavePublishedTrimmedSetPolicy> logger
    )
        : base(propertyName: "PublishTrimmed", requiredValue: "true", logger: logger) { }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        // if (!project.IsPublishable())
        // {
        //     return false;
        // }
        //
        // string? value = project.GetProperty("EnableTrimAnalyzer");
        //
        // return !string.IsNullOrWhiteSpace(value) && StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "true");
        return false;
    }
}
