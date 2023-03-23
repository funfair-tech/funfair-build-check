using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the include symbols is set appropriately.
/// </summary>
public sealed class IncludeSymbolsPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<IncludeSymbolsPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public IncludeSymbolsPolicy(ILogger<IncludeSymbolsPolicy> logger)
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

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IncludeSymbols", requiredValue: EXPECTED, logger: this._logger);
    }
}