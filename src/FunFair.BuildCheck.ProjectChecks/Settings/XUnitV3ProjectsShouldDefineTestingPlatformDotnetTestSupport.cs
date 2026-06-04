using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport : IProjectCheck
{
    private const string PROPERTY_NAME = "TestingPlatformDotnetTestSupport";

    private readonly ILogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> _logger;

    public XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport(
        ILogger<XUnitV3ProjectsShouldDefineTestingPlatformDotnetTestSupport> logger
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
            string? isTestingPlatformApplication = project.GetProperty("IsTestingPlatformApplication");
            bool suppressesTestDiscovery =
                isTestingPlatformApplication is not null
                && StringComparer.OrdinalIgnoreCase.Equals(x: isTestingPlatformApplication, y: "false");

            if (project.HasProperty(PROPERTY_NAME) && !suppressesTestDiscovery)
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
