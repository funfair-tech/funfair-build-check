using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ImportCommonProps : IProjectCheck
{
    private readonly ILogger<ImportCommonProps> _logger;
    private readonly string _projectImport;

    public ImportCommonProps(IRepositorySettings repositorySettings, ILogger<ImportCommonProps> logger)
    {
        this._logger = logger;

        this._projectImport = repositorySettings.ProjectImport;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._projectImport))
        {
            return ValueTask.CompletedTask;
        }

        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        bool found = project.HasProjectImport(this._projectImport);

        if (!found)
        {
            this._logger.PackableProjectShouldImportProject(projectName, this._projectImport);
        }

        return ValueTask.CompletedTask;
    }
}