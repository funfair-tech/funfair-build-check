using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy : IProjectCheck
{
    private readonly ILogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> _logger;

    public MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy(
        ILogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger
    )
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        string? publishAot = project.GetProperty("PublishAot");
        string? publishReadyToRun = project.GetProperty("PublishReadyToRun");

        bool aotTrue = StringComparer.OrdinalIgnoreCase.Equals(x: publishAot, y: "true");
        bool readyToRunTrue = StringComparer.OrdinalIgnoreCase.Equals(x: publishReadyToRun, y: "true");

        if (aotTrue && readyToRunTrue)
        {
            this._logger.CannotSpecifyBothPublishAotAndPublishReadyToRun(project.Name);
        }

        return ValueTask.CompletedTask;
    }
}
