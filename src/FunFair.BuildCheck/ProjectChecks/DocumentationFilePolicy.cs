using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DocumentationFilePolicy : IProjectCheck
    {
        private const string EXPECTED = @"bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml";

        private readonly ILogger<AnalysisLevelPolicyUseLatestVersion> _logger;

        public DocumentationFilePolicy(ILogger<AnalysisLevelPolicyUseLatestVersion> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            bool testProject = project.IsTestProject(projectName: projectName, logger: this._logger);

            if (testProject && Classifications.IsUnitTestBase())
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
            XmlNodeList nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

            if (nodes.Count != 0)
            {
                this._logger.LogError($"{projectName}: Test projects should not have XML Documentation");
            }
        }
    }
}