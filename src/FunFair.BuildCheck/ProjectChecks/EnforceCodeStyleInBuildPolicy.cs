using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class EnforceCodeStyleInBuildPolicy : IProjectCheck
    {
        private const string EXPECTED = @"true";

        private readonly ILogger<EnforceCodeStyleInBuildPolicy> _logger;

        public EnforceCodeStyleInBuildPolicy(ILogger<EnforceCodeStyleInBuildPolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"EnforceCodeStyleInBuild", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}