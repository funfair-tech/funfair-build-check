using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that Use System Resource Keys is enabled
/// </summary>
public sealed class UseSystemResourceKeysPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<UseSystemResourceKeysPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UseSystemResourceKeysPolicy(ILogger<UseSystemResourceKeysPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"UseSystemResourceKeys", requiredValue: EXPECTED, logger: this._logger);
    }
}