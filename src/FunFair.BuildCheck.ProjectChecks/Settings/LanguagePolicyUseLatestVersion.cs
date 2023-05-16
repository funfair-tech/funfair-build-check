using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class LanguagePolicyUseLatestVersion : IProjectCheck
{
    private readonly ILogger<LanguagePolicyUseLatestVersion> _logger;

    public LanguagePolicyUseLatestVersion(ILogger<LanguagePolicyUseLatestVersion> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"LangVersion", requiredValue: "latest", logger: this._logger);
    }
}