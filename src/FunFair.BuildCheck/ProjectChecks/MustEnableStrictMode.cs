using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class MustEnableStrictMode : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustEnableStrictMode(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName, project, nodePresence: @"Features", requiredValue: "strict;flow-analysis", this._logger);
        }
    }
}