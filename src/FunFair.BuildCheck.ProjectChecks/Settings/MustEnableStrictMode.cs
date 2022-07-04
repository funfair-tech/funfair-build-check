using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks the feature policy that 'strict' and 'flow-analysis' are enabled.
/// </summary>
public sealed class MustEnableStrictMode : IProjectCheck
{
    private readonly ILogger<MustEnableStrictMode> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustEnableStrictMode(ILogger<MustEnableStrictMode> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"Features", requiredValue: "strict;flow-analysis", logger: this._logger);
    }
}