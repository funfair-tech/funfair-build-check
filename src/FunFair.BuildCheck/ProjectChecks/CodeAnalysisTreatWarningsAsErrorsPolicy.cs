using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class CodeAnalysisTreatWarningsAsErrorsPolicy : IProjectCheck
    {
        private const string EXPECTED = @"true";

        private readonly ILogger<CodeAnalysisTreatWarningsAsErrorsPolicy> _logger;

        public CodeAnalysisTreatWarningsAsErrorsPolicy(ILogger<CodeAnalysisTreatWarningsAsErrorsPolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"CodeAnalysisTreatWarningsAsErrors", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}