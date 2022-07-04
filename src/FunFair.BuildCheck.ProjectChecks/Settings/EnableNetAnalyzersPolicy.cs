using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the .net analyzers are enabled appropriately.
/// </summary>
public sealed class EnableNetAnalyzersPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<EnableNetAnalyzersPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public EnableNetAnalyzersPolicy(ILogger<EnableNetAnalyzersPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"EnableNETAnalyzers", requiredValue: EXPECTED, logger: this._logger);
    }
}