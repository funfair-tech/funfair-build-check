﻿using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class NuGetPolicyDisableImplicitNuGetFallbackFolder : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public NuGetPolicyDisableImplicitNuGetFallbackFolder(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName,
                                           project: project,
                                           nodePresence: @"DisableImplicitNuGetFallbackFolder",
                                           requiredValue: "true",
                                           logger: this._logger);
        }
    }
}