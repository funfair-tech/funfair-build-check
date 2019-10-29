using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class MustUseOpenApiAnalyzers : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustUseOpenApiAnalyzers(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            // Not ready for tests yet
#if READY_FOR_CHECKS
            // TODO: Enable when ready
            ProjectValueHelpers.CheckValue(projectName, project, nodePresence: @"IncludeOpenAPIAnalyzers", requiredValue: "true", this._logger);
#endif
        }
    }
}