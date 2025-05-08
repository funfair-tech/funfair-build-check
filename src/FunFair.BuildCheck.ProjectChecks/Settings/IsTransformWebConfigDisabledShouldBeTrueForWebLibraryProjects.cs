using System;
using System.Threading;
using System.Threading.Tasks;
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

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        string sdk = project.GetSdk();

        if (!StringComparer.OrdinalIgnoreCase.Equals(x: sdk, y: "Microsoft.NET.Sdk.Web"))
        {
            // not a web project
            return ValueTask.CompletedTask;
        }

        if (!StringComparer.OrdinalIgnoreCase.Equals(project.GetOutputType(), y: "Library"))
        {
            // not a library
            return ValueTask.CompletedTask;
        }

        ProjectValueHelpers.CheckValue(project: project, nodePresence: "IsTransformWebConfigDisabled", requiredValue: true, logger: this._logger);

        return ValueTask.CompletedTask;
    }
}