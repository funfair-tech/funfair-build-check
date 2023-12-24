using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects : IProjectCheck
{
    private readonly ILogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> _logger;

    public IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects(ILogger<IsTransformWebConfigDisabledShouldBeTrueForWebLibraryProjects> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        string sdk = project.GetSdk();

        if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: sdk, y: "Microsoft.NET.Sdk.Web"))
        {
            // not a web project
            return ValueTask.CompletedTask;
        }

        if (!StringComparer.InvariantCultureIgnoreCase.Equals(project.GetOutputType(), y: "Library"))
        {
            // not a library
            return ValueTask.CompletedTask;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "IsTransformWebConfigDisabled", requiredValue: true, logger: this._logger);

        return ValueTask.CompletedTask;
    }
}