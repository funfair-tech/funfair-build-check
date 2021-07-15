using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that libraries are 'packable' except when they are test assemblies.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class OnlyExesShouldBePublishablePolicy : IProjectCheck
    {
        private readonly bool _isUnitTestBase;
        private readonly ILogger<OnlyExesShouldBePublishablePolicy> _logger;

        private readonly Func<bool, bool, bool, string, bool> _packablePolicy;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="repositorySettings">Repository settings.</param>
        /// <param name="logger">Logging.</param>
        public OnlyExesShouldBePublishablePolicy(IRepositorySettings repositorySettings, ILogger<OnlyExesShouldBePublishablePolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._isUnitTestBase = repositorySettings.IsUnitTestBase;

            string packable = Environment.GetEnvironmentVariable(variable: @"DOTNET_PUBLISHABLE") ?? "NONE";

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "NONE"))
            {
                this._packablePolicy = (_, _, _, _) => false;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "ALL"))
            {
                this._packablePolicy = (_, _, isTestProject, _) => !isTestProject;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "EXE_TOOL"))
            {
                this._packablePolicy = (isDotNetTool, isExe, isTestProject, _) => (isExe || isDotNetTool) && !isTestProject;
            }
            else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "EXE"))
            {
                this._packablePolicy = (_, isExe, isTestProject, _) => isExe && !isTestProject;
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

            bool isExe = StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", project.GetOutputType());

            bool packable = this._packablePolicy(arg1: isDotNetTool, arg2: isExe, arg3: isTestProject, arg4: projectName);

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IsPublishable", requiredValue: packable, logger: this._logger);
        }
    }
}