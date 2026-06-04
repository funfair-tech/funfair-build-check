using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XUnitV3ProjectsShouldDefineIsTestingPlatformApplication : IProjectCheck
{
    private const string PROPERTY_NAME = "IsTestingPlatformApplication";

    private readonly ILogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> _logger;

    public XUnitV3ProjectsShouldDefineIsTestingPlatformApplication(
        ILogger<XUnitV3ProjectsShouldDefineIsTestingPlatformApplication> logger
    )
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (
            project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(x: project.GetOutputType(), y: "Exe")
        )
        {
            return ValueTask.CompletedTask;
        }

        string? isTestProject = project.GetProperty("IsTestProject");

        if (isTestProject is not null && StringComparer.OrdinalIgnoreCase.Equals(x: isTestProject, y: "false"))
        {
            string? value = project.GetProperty(PROPERTY_NAME);

            if (value is not null && !StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "false"))
            {
                this._logger.ProjectShouldNotDefineProperty(project.Name, PROPERTY_NAME);
            }

            return ValueTask.CompletedTask;
        }

        if (
            project.IsTestProject(logger: this._logger)
            && (
                project.ReferencesPackage("xunit.v3", this._logger)
                || project.ReferencesPackage("xunit.v3.extensibility.core", this._logger)
            )
        )
        {
            ProjectValueHelpers.CheckValue(
                project: project,
                nodePresence: PROPERTY_NAME,
                requiredValue: "true",
                logger: this._logger
            );
        }

        return ValueTask.CompletedTask;
    }
}
