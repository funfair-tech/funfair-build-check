using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class JsonSerializerIsReflectionEnabledByDefaultPolicy : IProjectCheck
{
    private const string EXPECTED = "false";

    private readonly ILogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> _logger;

    public JsonSerializerIsReflectionEnabledByDefaultPolicy(ILogger<JsonSerializerIsReflectionEnabledByDefaultPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: "JsonSerializerIsReflectionEnabledByDefault",
                                       requiredValue: EXPECTED,
                                       logger: this._logger);
    }
}