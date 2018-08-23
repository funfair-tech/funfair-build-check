using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class ErrorPolicyWarningAsErrors : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public ErrorPolicyWarningAsErrors(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckNode(projectName, project, nodePresence: @"WarningsAsErrors", this._logger);

            ProjectValueHelpers.CheckValue(projectName, project, nodePresence: @"TreatWarningsAsErrors", requiredValue: true, this._logger);
        }
    }
}