using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XmlDocumentationFileProhibitedPolicy : IProjectCheck
{
    private readonly ILogger<XmlDocumentationFileProhibitedPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public XmlDocumentationFileProhibitedPolicy(IRepositorySettings repositorySettings, ILogger<XmlDocumentationFileProhibitedPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (this._repositorySettings.XmlDocumentationRequired)
        {
            return ValueTask.CompletedTask;
        }

        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

        if (nodes is not null && nodes.Count != 0)
        {
            this._logger.ShouldNotHaveXmlDocumentation(projectName);
        }

        return ValueTask.CompletedTask;
    }
}