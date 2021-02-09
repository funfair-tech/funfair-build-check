using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that libraries do not depend on executables.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class LibrariesShouldNotDependOnExecutables : IProjectCheck
    {
        private readonly ILogger<LibrariesShouldNotDependOnExecutables> _logger;
        private readonly IProjectLoader _projectLoader;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="projectLoader">Project loader.</param>
        /// <param name="logger">Logging.</param>
        public LibrariesShouldNotDependOnExecutables(IProjectLoader projectLoader, ILogger<LibrariesShouldNotDependOnExecutables> logger)
        {
            this._projectLoader = projectLoader ?? throw new ArgumentNullException(nameof(projectLoader));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: "Library", project.GetOutputType()))
            {
                return;
            }

            XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/ProjectReference");

            if (nodes == null)
            {
                return;
            }

            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                string projectReference = reference.GetAttribute(name: "Include");

                string referencedProject = Path.Combine(path1: projectFolder, path2: projectReference);
                FileInfo i = new(referencedProject);

                if (!i.Exists)
                {
                    continue;
                }

                referencedProject = i.FullName;

                XmlDocument otherProject = this._projectLoader.Load(referencedProject);

                string otherOutputType = otherProject.GetOutputType();

                if (StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", y: otherOutputType))
                {
                    this._logger.LogError($"Library project {projectName} references exe {referencedProject}");
                }
            }
        }
    }
}