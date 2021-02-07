using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that the compiler analysis level policy is set to latest.
    /// </summary>    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class AnalysisLevelPolicyUseLatestVersion : IProjectCheck
    {
        private readonly ILogger<AnalysisLevelPolicyUseLatestVersion> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public AnalysisLevelPolicyUseLatestVersion(ILogger<AnalysisLevelPolicyUseLatestVersion> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"AnalysisLevel", requiredValue: "latest", logger: this._logger);
        }
    }
}
