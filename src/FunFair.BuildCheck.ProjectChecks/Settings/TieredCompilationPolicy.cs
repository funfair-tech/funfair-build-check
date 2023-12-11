using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TieredCompilationPolicy : IProjectCheck
{
    private const string EXPECTED = "true";

    private readonly ILogger<TieredCompilationPolicy> _logger;

    public TieredCompilationPolicy(ILogger<TieredCompilationPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPublishable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "TieredCompilation", requiredValue: EXPECTED, logger: this._logger);
    }
}