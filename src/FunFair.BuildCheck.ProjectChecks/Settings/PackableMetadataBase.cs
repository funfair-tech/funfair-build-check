using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks for metadata being set on packable projects
    /// </summary>
    public abstract class PackableMetadataBase : IProjectCheck
    {
        private readonly ILogger _logger;
        private readonly string _property;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="property">The property that needs to be set.</param>
        /// <param name="logger">Logging.</param>
        protected PackableMetadataBase(string property, ILogger logger)
        {
            this._property = property ?? throw new ArgumentNullException(nameof(property));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (!project.IsPackable())
            {
                return;
            }

            if (!project.HasProperty(this._property))
            {
                this._logger.LogError($"Packable project {projectName} does not define {this._property}");
            }
        }
    }
}