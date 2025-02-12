using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TieredCompilationPolicy : SimplePropertyProjectCheckBase
{
    public TieredCompilationPolicy(ILogger<TieredCompilationPolicy> logger)
        : base(propertyName: "TieredCompilation", requiredValue: "true", logger: logger) { }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPublishable();
    }
}
