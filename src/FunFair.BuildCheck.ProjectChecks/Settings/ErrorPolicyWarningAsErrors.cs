using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ErrorPolicyWarningAsErrors : IProjectCheck
{
    private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

    public ErrorPolicyWarningAsErrors(ILogger<ErrorPolicyWarningAsErrors> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckNode(projectName: projectName, project: project, nodePresence: @"WarningsAsErrors", logger: this._logger);

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TreatWarningsAsErrors", requiredValue: true, logger: this._logger);
    }
}