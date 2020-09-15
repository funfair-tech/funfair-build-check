using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
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
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"DocumentationFile", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}