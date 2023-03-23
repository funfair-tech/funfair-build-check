using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that extended analyzer rules is set appropriately.
/// </summary>
public sealed class EnforceExtendedAnalyzerRulesPolicy : IProjectCheck
{
    private readonly ILogger<EnforceExtendedAnalyzerRulesPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public EnforceExtendedAnalyzerRulesPolicy(ILogger<EnforceExtendedAnalyzerRulesPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"EnforceExtendedAnalyzerRules",
                                       requiredValue: project.IsAnalyzerOrSourceGenerator() ? "true" : "false",
                                       logger: this._logger);
    }
}