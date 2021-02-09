using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
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
        private readonly bool _isUnitTestBase;
        private readonly ILogger<LibrariesShouldBePackablePolicy> _logger;

        private readonly Func<bool, bool, bool, string, bool> _packablePolicy;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public LibrariesShouldBePackablePolicy(ILogger<LibrariesShouldBePackablePolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._isUnitTestBase = Classifications.IsUnitTestBase();

            string packable = Environment.GetEnvironmentVariable(variable: @"DOTNET_PACKABLE") ?? "NONE";

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "NONE"))
            {
                this._packablePolicy = (_, _, _, _) => false;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "ALL"))
            {
                this._packablePolicy = (_, _, isTestProject, _) => !isTestProject;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "LIBRARIES"))
            {
                this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => isLibrary && !isTestProject && !isDotNetTool;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "LIBRARY_TOOL"))
            {
                this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => (isLibrary || isDotNetTool) && !isTestProject;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "TOOLS"))
            {
                this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => !isLibrary && !isTestProject && isDotNetTool;
            }
            else
            {
                ImmutableHashSet<string> projects = packable.Split(",")
                                                            .Select(p => p.Trim())
                                                            .Where(p => !string.IsNullOrWhiteSpace(p))
                                                            .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

                this._packablePolicy = (_, _, isTestProject, projectName) => !isTestProject && projects.Contains(projectName);
            }
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            bool isTestProject = project.IsTestProject(projectName: projectName, logger: this._logger) &&
                                 (this._isUnitTestBase && projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase) || !this._isUnitTestBase);

            bool isDotNetTool = project.IsDotNetTool();

            bool isLibrary = StringComparer.InvariantCultureIgnoreCase.Equals(x: "Library", project.GetOutputType());

            bool packable = this._packablePolicy(arg1: isDotNetTool, arg2: isLibrary, arg3: isTestProject, arg4: projectName);

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IsPackable", requiredValue: packable, logger: this._logger);
        }
    }
}