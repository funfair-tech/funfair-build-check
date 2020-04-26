using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class MustSpecifyOutputType : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustSpecifyOutputType(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            const string msg = "Exe or Library";

            static bool IsRequiredValue(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                return StringComparer.InvariantCultureIgnoreCase.Equals(value, y: "Exe") || StringComparer.InvariantCultureIgnoreCase.Equals(value, y: "Library");
            }

            ProjectValueHelpers.CheckValue(projectName, project, nodePresence: @"OutputType", IsRequiredValue, msg, this._logger);
        }
    }
}