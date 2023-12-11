using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ImplicitUsingsPolicy : IProjectCheck
{
    private const string EXPECTED = "disable";

    private readonly ILogger<ImplicitUsingsPolicy> _logger;

    public ImplicitUsingsPolicy(ILogger<ImplicitUsingsPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "ImplicitUsings", requiredValue: EXPECTED, logger: this._logger);
    }
}