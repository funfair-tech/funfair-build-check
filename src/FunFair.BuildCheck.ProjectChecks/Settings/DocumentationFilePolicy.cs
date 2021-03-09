using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that the DocumentationFile is set appropriately
    /// </summary>


    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class DocumentationFilePolicy : IProjectCheck
    {
        private const string EXPECTED = @"bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml";
        private readonly ILogger<DocumentationFilePolicy> _logger;

        private readonly IRepositorySettings _repositorySettings;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="repositorySettings">Repository settings.</param>
        /// <param name="logger">Logging.</param>
        public DocumentationFilePolicy(IRepositorySettings repositorySettings, ILogger<DocumentationFilePolicy> logger)
        {
            this._repositorySettings = repositorySettings ?? throw new ArgumentNullException(nameof(repositorySettings));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            bool testProject = project.IsTestProject(projectName: projectName, logger: this._logger);

            if (testProject && this._repositorySettings.IsUnitTestBase)
            {
                testProject = projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
            }

            if (testProject)
            {
                this.CheckTestProject(projectName: projectName, project: project);

                return;
            }

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"DocumentationFile", requiredValue: EXPECTED, logger: this._logger);
        }

        private void CheckTestProject(string projectName, XmlDocument project)
        {
            XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

            if (nodes != null && nodes.Count != 0)
            {
                this._logger.LogError($"{projectName}: Test projects should not have XML Documentation");
            }
        }
    }
}

