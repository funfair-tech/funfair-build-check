using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class OptimizationPreferencePolicy : SimplePropertyProjectCheckBase
{
    public OptimizationPreferencePolicy(ILogger<OptimizationPreferencePolicy> logger)
        : base(propertyName: "OptimizationPreference", requiredValue: "speed", logger: logger) { }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPackable();
    }
}
