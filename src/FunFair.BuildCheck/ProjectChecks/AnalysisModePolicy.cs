using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class AnalysisModePolicy : IProjectCheck
    {
        private const string EXPECTED = @"AllEnabledByDefault";

        private readonly ILogger<AnalysisModePolicy> _logger;

        public AnalysisModePolicy(ILogger<AnalysisModePolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"AnalysisMode", requiredValue: EXPECTED, logger: this._logger);
        }
    }

    public sealed class LibrariesShouldBePackablePolicy : IProjectCheck
    {
        private readonly ILogger<LibrariesShouldBePackablePolicy> _logger;

        public LibrariesShouldBePackablePolicy(ILogger<LibrariesShouldBePackablePolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: "Library", project.GetOutputType()))
            {
                return;
            }

            bool packable = !(project.IsTestProject(projectName: projectName, logger: this._logger) && !Classifications.IsUnitTestBase());

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IsPackable", requiredValue: packable, logger: this._logger);
        }
    }
}