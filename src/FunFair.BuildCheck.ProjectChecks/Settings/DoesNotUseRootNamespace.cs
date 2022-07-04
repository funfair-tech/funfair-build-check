using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the project does not include a RootNamespace setting.
/// </summary>
public sealed class DoesNotUseRootNamespace : IProjectCheck
{
    private readonly ILogger<DoesNotUseRootNamespace> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public DoesNotUseRootNamespace(ILogger<DoesNotUseRootNamespace> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/RootNamespace");

        if (nodes == null)
        {
            return;
        }

        if (nodes.Count == 0)
        {
            return;
        }

        this._logger.LogError($"{projectName} Uses the RootNamepace setting to rename the namespace, when it should not");
    }
}