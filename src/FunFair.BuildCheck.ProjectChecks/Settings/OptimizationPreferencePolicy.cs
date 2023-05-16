using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class OptimizationPreferencePolicy : IProjectCheck
{
    private const string EXPECTED = @"speed";

    private readonly ILogger<OptimizationPreferencePolicy> _logger;

    public OptimizationPreferencePolicy(ILogger<OptimizationPreferencePolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"OptimizationPreference", requiredValue: EXPECTED, logger: this._logger);
    }
}