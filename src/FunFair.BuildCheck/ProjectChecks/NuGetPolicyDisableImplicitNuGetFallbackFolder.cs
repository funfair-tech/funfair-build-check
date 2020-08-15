﻿using System;
using System.Xml;

namespace FunFair.BuildCheck.ProjectChecks
{
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