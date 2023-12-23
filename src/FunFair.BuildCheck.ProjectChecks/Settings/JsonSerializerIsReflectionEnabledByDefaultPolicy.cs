using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class JsonSerializerIsReflectionEnabledByDefaultPolicy : SimplePropertyProjectCheckBase
{
    public JsonSerializerIsReflectionEnabledByDefaultPolicy(ILogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> logger)
        : base(propertyName: "JsonSerializerIsReflectionEnabledByDefault", requiredValue: "false", logger: logger)
    {
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPackable();
    }
}