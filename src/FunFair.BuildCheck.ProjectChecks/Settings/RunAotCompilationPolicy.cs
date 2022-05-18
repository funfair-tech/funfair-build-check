using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that AOT Compilation is enabled appropriately.
/// </summary>
public sealed class RunAotCompilationPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<RunAotCompilationPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public RunAotCompilationPolicy(ILogger<RunAotCompilationPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"RunAOTCompilation", requiredValue: EXPECTED, logger: this._logger);
    }
}