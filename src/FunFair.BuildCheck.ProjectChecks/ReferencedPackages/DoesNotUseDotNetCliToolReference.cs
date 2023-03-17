using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that a project does not use the Dot Net CLi Tool Reference.
/// </summary>
public sealed class DoesNotUseDotNetCliToolReference : IProjectCheck
{
    private readonly ILogger<DoesNotUseDotNetCliToolReference> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public DoesNotUseDotNetCliToolReference(ILogger<DoesNotUseDotNetCliToolReference> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/DotNetCliToolReference");

        if (nodes != null && nodes.Count != 0)
        {
            this._logger.ContainsDotNetCliToolReference(projectName);
        }
    }
}