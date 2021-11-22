using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that the package validation is set appropriately.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class EnablePackageValidationPolicy : IProjectCheck
    {
        private const string EXPECTED = @"true";

        private readonly ILogger<EnablePackageValidationPolicy> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public EnablePackageValidationPolicy(ILogger<EnablePackageValidationPolicy> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (!project.IsPackable())
            {
                return;
            }

            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"EnablePackageValidation", requiredValue: EXPECTED, logger: this._logger);
        }
    }
}