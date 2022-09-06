using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that Debugger Support is defined
/// </summary>
public sealed class DebuggerSupportPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<DebuggerSupportPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public DebuggerSupportPolicy(ILogger<DebuggerSupportPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"DebuggerSupport", requiredValue: EXPECTED, logger: this._logger);
    }
}