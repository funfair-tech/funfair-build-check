using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that the implicit fallback folder for nuget is turned off.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class NuGetPolicyDisableImplicitNuGetFallbackFolder : IProjectCheck
    {
        private readonly ILogger<NuGetPolicyDisableImplicitNuGetFallbackFolder> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public NuGetPolicyDisableImplicitNuGetFallbackFolder(ILogger<NuGetPolicyDisableImplicitNuGetFallbackFolder> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"DisableImplicitNuGetFallbackFolder", requiredValue: "true", logger: this._logger);
        }
    }
}