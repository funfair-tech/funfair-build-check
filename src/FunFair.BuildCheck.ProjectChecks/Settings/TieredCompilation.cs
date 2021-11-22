using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that the tiered compilation property is set appropriately.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class TieredCompilationPolicy : IProjectCheck
    {
        private const string EXPECTED = @"true";

        private readonly ILogger<TieredCompilationPolicy> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public TieredCompilationPolicy(ILogger<TieredCompilationPolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (!project.IsPublishable())
            {
                return;
            }

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"TieredCompilation", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}