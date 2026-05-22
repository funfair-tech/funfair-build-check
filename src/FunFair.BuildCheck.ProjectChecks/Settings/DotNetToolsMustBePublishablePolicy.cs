using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class DotNetToolsMustBePublishablePolicy : IProjectCheck
{
    private readonly ILogger<DotNetToolsMustBePublishablePolicy> _logger;

    public DotNetToolsMustBePublishablePolicy(ILogger<DotNetToolsMustBePublishablePolicy> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!IsToolProject(project))
        {
            return ValueTask.CompletedTask;
        }

        if (!project.IsPublishable())
        {
            this._logger.MustBePublishable(project.Name);
        }

        return ValueTask.CompletedTask;
    }

    private static bool IsToolProject(in ProjectContext project)
    {
        return project.IsDotNetTool()
            || StringComparer.OrdinalIgnoreCase.Equals(x: project.GetProperty("IsTool"), y: "true")
            || project.HasProperty("ToolCommandName");
    }
}
