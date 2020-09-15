using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class LanguagePolicyUseLatestVersion : IProjectCheck
    {
        private readonly ILogger<LanguagePolicyUseLatestVersion> _logger;

        public LanguagePolicyUseLatestVersion(ILogger<LanguagePolicyUseLatestVersion> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"LangVersion", requiredValue: "latest", logger: this._logger);
        }
    }
}