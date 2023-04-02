using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IlcOptimizationPreferencePolicy : IProjectCheck
{
    private const string EXPECTED = @"Size";

    private readonly ILogger<IlcOptimizationPreferencePolicy> _logger;

    public IlcOptimizationPreferencePolicy(ILogger<IlcOptimizationPreferencePolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IlcOptimizationPreference", requiredValue: EXPECTED, logger: this._logger);
    }
}