using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
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

            bool packable = !project.IsTestProject(projectName: projectName, logger: this._logger);

            if (!packable && Classifications.IsUnitTestBase())
            {
                packable = !projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
            }

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IsPackable", requiredValue: packable, logger: this._logger);
        }
    }
}