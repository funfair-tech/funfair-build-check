using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ErrorPolicyWarningAsErrors : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public ErrorPolicyWarningAsErrors(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckNode(projectName: projectName, project: project, nodePresence: @"WarningsAsErrors", logger: this._logger);

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TreatWarningsAsErrors", requiredValue: true, logger: this._logger);
        }
    }
}