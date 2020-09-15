using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class AnalysisLevelPolicyUseLatestVersion : IProjectCheck
    {
        private readonly ILogger<AnalysisLevelPolicyUseLatestVersion> _logger;

        public AnalysisLevelPolicyUseLatestVersion(ILogger<AnalysisLevelPolicyUseLatestVersion> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"AnalysisLevel", requiredValue: "latest", logger: this._logger);
        }
    }
}