using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks to see if the TargetFramework/TargetFrameworks is set correctly.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class TargetFrameworkIsSetCorrectlyPolicy : IProjectCheck
    {
        private readonly IRepositorySettings _repositorySettings;
        private readonly ILogger<TargetFrameworkIsSetCorrectlyPolicy> _logger;
        private readonly string? _expected;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="repositorySettings">Repository settings</param>
        /// <param name="logger">Logging.</param>
        public TargetFrameworkIsSetCorrectlyPolicy(IRepositorySettings repositorySettings, ILogger<TargetFrameworkIsSetCorrectlyPolicy> logger)
        {
            this._repositorySettings = repositorySettings;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._expected = Environment.GetEnvironmentVariable("DOTNET_CORE_APP_TARGET_FRAMEWORK");
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (this._repositorySettings.IsCodeAnalysisSolution && !project.IsTestProject(projectName: projectName, logger: this._logger))
            {
                // Code analysis project has specific requirements
                ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TargetFramework", requiredValue: "netstandard2.0", logger: this._logger);

                return;
            }

            if (string.IsNullOrEmpty(this._expected))
            {
                // no frameworks defined - allow any
                return;
            }

            string[] frameworks = this._expected.Split(";");

            switch (frameworks.Length)
            {
                case 0: return;
                case 1:
                    ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TargetFramework", frameworks[0], logger: this._logger);

                    break;
                default:
                    ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TargetFrameworks", requiredValue: this._expected, logger: this._logger);

                    break;
            }
        }
    }
}