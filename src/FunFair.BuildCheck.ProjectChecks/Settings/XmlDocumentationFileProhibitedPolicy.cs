using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the DocumentationFile is set appropriately
/// </summary>
public sealed class XmlDocumentationFileProhibitedPolicy : IProjectCheck
{
    private readonly ILogger<XmlDocumentationFileProhibitedPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public XmlDocumentationFileProhibitedPolicy(IRepositorySettings repositorySettings, ILogger<XmlDocumentationFileProhibitedPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (this._repositorySettings.XmlDocumentationRequired)
        {
            return;
        }

        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

        if (nodes != null && nodes.Count != 0)
        {
            this._logger.LogError($"{projectName}: Should not have XML Documentation");
        }
    }
}