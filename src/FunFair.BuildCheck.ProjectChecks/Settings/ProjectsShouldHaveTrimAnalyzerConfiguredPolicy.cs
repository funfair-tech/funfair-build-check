using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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

    public ValueTask CheckAsync(
        string projectName,
        string projectFolder,
        XmlDocument project,
        CancellationToken cancellationToken
    )
    {
        if (!project.HasProperty(SETTING))
        {
            this._logger.ProjectShouldConfigureTrimAnalyzer(
                projectName: projectName,
                property: SETTING
            );
        }

        return ValueTask.CompletedTask;
    }
}
