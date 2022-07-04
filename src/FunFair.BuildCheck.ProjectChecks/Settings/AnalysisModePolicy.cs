using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     The checks analysis mode policy is set appropriately.
/// </summary>
public sealed class AnalysisModePolicy : IProjectCheck
{
    private const string EXPECTED = @"AllEnabledByDefault";

    private readonly ILogger<AnalysisModePolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public AnalysisModePolicy(ILogger<AnalysisModePolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"AnalysisMode", requiredValue: EXPECTED, logger: this._logger);
    }
}