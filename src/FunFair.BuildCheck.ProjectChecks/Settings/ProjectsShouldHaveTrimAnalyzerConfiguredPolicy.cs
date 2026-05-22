using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ProjectsShouldHaveTrimAnalyzerConfiguredPolicy : IProjectCheck
{
    private const string SETTING = "EnableTrimAnalyzer";
    private const string IS_TRIMMABLE = "IsTrimmable";

    private readonly ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> _logger;

    public ProjectsShouldHaveTrimAnalyzerConfiguredPolicy(
        ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger
    )
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        string? trimAnalyzer = project.GetProperty(SETTING);

        if (string.IsNullOrWhiteSpace(trimAnalyzer))
        {
            this._logger.ProjectShouldConfigureTrimAnalyzer(projectName: project.Name, property: SETTING);

            return ValueTask.CompletedTask;
        }

        string? isTrimmable = project.GetProperty(IS_TRIMMABLE);
        bool trimmable = StringComparer.OrdinalIgnoreCase.Equals(x: isTrimmable, y: "true");

        if (trimmable && !StringComparer.OrdinalIgnoreCase.Equals(x: trimAnalyzer, y: "true"))
        {
            this._logger.ProjectMustEnableTrimAnalyzerWhenTrimmable(projectName: project.Name, property: SETTING);
        }

        return ValueTask.CompletedTask;
    }
}
