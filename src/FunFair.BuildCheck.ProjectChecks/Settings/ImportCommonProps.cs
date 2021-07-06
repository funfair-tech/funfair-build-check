using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Looks for common properties project import.
    /// </summary>
    public sealed class ImportCommonProps : IProjectCheck
    {
        private readonly ILogger<ImportCommonProps> _logger;
        private readonly string _projectImport;

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
                foreach (XmlElement import in imports.OfType<XmlElement>())
                {
                    string projectImport = import.GetAttribute(name: "Project");

                    if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._projectImport, y: projectImport))
                    {
                        found = true;

                        break;
                    }
                }
            }

            if (!found)
            {
                this._logger.LogError($"Packable project {projectName} should <Import Project=\"{this._projectImport}\" />");
            }
        }
    }
}