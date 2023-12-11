using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class UseSystemResourceKeysPolicy : IProjectCheck
{
    private const string EXPECTED = "true";

    private readonly ILogger<UseSystemResourceKeysPolicy> _logger;

    public UseSystemResourceKeysPolicy(ILogger<UseSystemResourceKeysPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "UseSystemResourceKeys", requiredValue: EXPECTED, logger: this._logger);
    }
}