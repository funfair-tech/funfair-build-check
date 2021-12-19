using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Looks for common properties project import.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class ImportCommonProps : IProjectCheck
{
    private readonly ILogger<ImportCommonProps> _logger;
    private readonly string _projectImport;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ImportCommonProps(ILogger<ImportCommonProps> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this._projectImport = Environment.GetEnvironmentVariable("DOTNET_PACK_PROJECT_METADATA_IMPORT") ?? string.Empty;
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

        bool found = false;
        XmlNodeList? imports = project.SelectNodes("/Project/Import[@Project]");

        if (imports != null)
        {
            found = imports.OfType<XmlElement>()
                           .Select(import => import.GetAttribute(name: "Project"))
                           .Any(projectImport => StringComparer.InvariantCultureIgnoreCase.Equals(x: this._projectImport, y: projectImport));
        }

        if (!found)
        {
            this._logger.LogError($"Packable project {projectName} should <Import Project=\"{this._projectImport}\" />");
        }
    }
}