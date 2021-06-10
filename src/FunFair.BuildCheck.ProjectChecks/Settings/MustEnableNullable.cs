using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that Nullable is turned on.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustEnableNullable : IProjectCheck
    {
        private readonly ILogger<MustEnableNullable> _logger;
        private readonly IRepositorySettings _repositorySettings;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="repositorySettings">Repository settings</param>
        /// <param name="logger">Logging.</param>
        public MustEnableNullable(IRepositorySettings repositorySettings, ILogger<MustEnableNullable> logger)
        {
            this._repositorySettings = repositorySettings ?? throw new ArgumentNullException(nameof(repositorySettings));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (!this._repositorySettings.IsNullableGloballyEnforced)
            {
                return;
            }

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"Nullable", requiredValue: "enable", logger: this._logger);
        }
    }
}