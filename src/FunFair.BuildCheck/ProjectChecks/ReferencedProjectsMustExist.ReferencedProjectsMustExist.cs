using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that libraries do not depend on executables.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ReferencedProjectsMustExist : IProjectCheck
    {
        private readonly ILogger<ReferencedProjectsMustExist> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ReferencedProjectsMustExist(ILogger<ReferencedProjectsMustExist> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            foreach (XmlElement reference in project.SelectNodes("/Project/ItemGroup/ProjectReference")
                                                    .OfType<XmlElement>())
            {
                string projectReference = reference.GetAttribute(name: "Include");

                string referencedProject = Path.Combine(path1: projectFolder, path2: projectReference);
                FileInfo i = new FileInfo(referencedProject);

                if (!i.Exists)
                {
                    this._logger.LogError($"Projecy {projectName} references {referencedProject} that does not exist.");
                }
            }
        }
    }
}