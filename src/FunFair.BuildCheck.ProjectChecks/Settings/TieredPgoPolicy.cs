using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TieredPgoPolicy : IProjectCheck
{
    private const string EXPECTED = "true";

    private readonly ILogger<TieredPgoPolicy> _logger;

    public TieredPgoPolicy(ILogger<TieredPgoPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "TieredPGO", requiredValue: EXPECTED, logger: this._logger);
    }
}