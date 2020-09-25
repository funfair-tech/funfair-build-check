﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that output type is explicitly specified.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustSpecifyOutputType : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public MustSpecifyOutputType(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            const string msg = "Exe or Library";

            static bool IsRequiredValue(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Exe") || StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Library");
            }

            ProjectValueHelpers.CheckValue(projectName: projectName,
                                           project: project,
                                           nodePresence: @"OutputType",
                                           isRequiredValue: IsRequiredValue,
                                           msg: msg,
                                           logger: this._logger);
        }
    }
}