using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EnableNetAnalyzersPolicy : IProjectCheck
    {
        private const string EXPECTED = @"true";

        private readonly ILogger<EnableNetAnalyzersPolicy> _logger;

        public EnableNetAnalyzersPolicy(ILogger<EnableNetAnalyzersPolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"EnableNETAnalyzers", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}