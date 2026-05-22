using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableExesMustHavePublishAotSetPolicy : IProjectCheck
{
    private readonly ILogger<PublishableExesMustHavePublishAotSetPolicy> _logger;

    public PublishableExesMustHavePublishAotSetPolicy(ILogger<PublishableExesMustHavePublishAotSetPolicy> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        bool isExe = StringComparer.OrdinalIgnoreCase.Equals(x: "Exe", project.GetOutputType());

        if (!isExe)
        {
            return ValueTask.CompletedTask;
        }

        bool isPublishable = project.IsPublishable();

        if (!isPublishable)
        {
            return ValueTask.CompletedTask;
        }

        string? publishAot = project.GetProperty("PublishAot");

        if (string.IsNullOrWhiteSpace(publishAot))
        {
            this._logger.ShouldDefinePublishAot(project.Name);
            return ValueTask.CompletedTask;
        }

        if (StringComparer.OrdinalIgnoreCase.Equals(x: publishAot, y: "true"))
        {
            string? isTrimmable = project.GetProperty("IsTrimmable");

            if (!StringComparer.OrdinalIgnoreCase.Equals(x: isTrimmable, y: "true"))
            {
                this._logger.CannotEnablePublishAotWhenNotTrimmable(project.Name);
            }
        }

        return ValueTask.CompletedTask;
    }
}
