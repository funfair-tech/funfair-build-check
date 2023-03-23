using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Looks for common properties project import.
/// </summary>
public sealed class ImportCommonProps : IProjectCheck
{
    private readonly ILogger<ImportCommonProps> _logger;
    private readonly string _projectImport;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ImportCommonProps(IRepositorySettings repositorySettings, ILogger<ImportCommonProps> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this._projectImport = repositorySettings.ProjectImport;
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (string.IsNullOrWhiteSpace(this._projectImport))
        {
            return;
        }

        if (!project.IsPackable())
        {
            return;
        }

        bool found = project.HasProjectImport(this._projectImport);

        if (!found)
        {
            this._logger.LogError($"Packable project {projectName} should <Import Project=\"{this._projectImport}\" />");
        }
    }
}