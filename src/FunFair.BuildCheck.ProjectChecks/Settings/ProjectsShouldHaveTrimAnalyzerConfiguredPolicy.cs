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

    private readonly ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> _logger;

    public ProjectsShouldHaveTrimAnalyzerConfiguredPolicy(
        ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger
    )
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.HasProperty(SETTING))
        {
            this._logger.ProjectShouldConfigureTrimAnalyzer(
                projectName: project.Name,
                property: SETTING
            );
        }

        return ValueTask.CompletedTask;
    }
}
