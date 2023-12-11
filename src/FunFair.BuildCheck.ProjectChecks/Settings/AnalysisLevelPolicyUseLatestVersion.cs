using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class AnalysisLevelPolicyUseLatestVersion : IProjectCheck
{
    private readonly ILogger<AnalysisLevelPolicyUseLatestVersion> _logger;

    public AnalysisLevelPolicyUseLatestVersion(ILogger<AnalysisLevelPolicyUseLatestVersion> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "AnalysisLevel", requiredValue: "latest", logger: this._logger);
    }
}