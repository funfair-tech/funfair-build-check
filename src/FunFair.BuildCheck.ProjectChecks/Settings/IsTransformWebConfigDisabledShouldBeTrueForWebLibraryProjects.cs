using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that IsTransformWebConfigDisabled is explicitly disabled for Libraries
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects : IProjectCheck
{
    private readonly ILogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> _logger;

    //        <IsTransformWebConfigDisabled>true</IsTransformWebConfigDisabled>

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects(ILogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        string sdk = project.GetSdk();

        if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: sdk, y: @"Microsoft.NET.Sdk.Web"))
        {
            // not a web project
            return;
        }

        if (!StringComparer.InvariantCultureIgnoreCase.Equals(project.GetOutputType(), y: "Library"))
        {
            // not a library
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IsTransformWebConfigDisabled", requiredValue: true, logger: this._logger);
    }
}