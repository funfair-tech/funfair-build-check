using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XmlDocumentationFileProhibitedPolicy : IProjectCheck
{
    private readonly ILogger<XmlDocumentationFileProhibitedPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public XmlDocumentationFileProhibitedPolicy(IRepositorySettings repositorySettings, ILogger<XmlDocumentationFileProhibitedPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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