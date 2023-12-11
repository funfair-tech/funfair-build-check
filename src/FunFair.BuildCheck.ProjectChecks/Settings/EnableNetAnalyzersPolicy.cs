using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnableNetAnalyzersPolicy : IProjectCheck
{
    private const string EXPECTED = "true";

    private readonly ILogger<EnableNetAnalyzersPolicy> _logger;

    public EnableNetAnalyzersPolicy(ILogger<EnableNetAnalyzersPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "EnableNETAnalyzers", requiredValue: EXPECTED, logger: this._logger);
    }
}