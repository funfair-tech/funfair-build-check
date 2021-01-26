using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that libraries are 'packable' except when they are test assemblies.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class LibrariesShouldBePackablePolicy : IProjectCheck
    {
        private readonly ILogger<LibrariesShouldBePackablePolicy> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public LibrariesShouldBePackablePolicy(ILogger<LibrariesShouldBePackablePolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
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