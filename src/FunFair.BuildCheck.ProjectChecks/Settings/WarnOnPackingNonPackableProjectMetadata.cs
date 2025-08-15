using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class WarnOnPackingNonPackableProjectMetadata : IProjectCheck
{
    private readonly ILogger<WarnOnPackingNonPackableProjectMetadata> _logger;

    public WarnOnPackingNonPackableProjectMetadata(ILogger<WarnOnPackingNonPackableProjectMetadata> logger)
    {
        this._logger = logger;
    }

    private const string PROPERTY_NAME = "WarnOnPackingNonPackableProject";

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        string notPresent = Guid.NewGuid()
                                .ToString();
        string disablePackingWarning = project.GetStringProperty("/Project/PropertyGroup/" + PROPERTY_NAME, notPresent);

        return project.IsPackable()
            ? this.CheckPackableAsync(project: project, disablePackingWarning: disablePackingWarning, notPresent: notPresent)
            : this.CheckNotPackableAsync(project: project, disablePackingWarning: disablePackingWarning, notPresent: notPresent);
    }

    private ValueTask CheckPackableAsync(in ProjectContext project, string disablePackingWarning, string notPresent)
    {
        if (!StringComparer.OrdinalIgnoreCase.Equals(disablePackingWarning, notPresent))
        {
            this._logger.PackableProjectDefinesNotWarnOnPackingNonPackableProjectToFalse(project.Name, PROPERTY_NAME, disablePackingWarning);
        }

        return ValueTask.CompletedTask;
    }

    private ValueTask CheckNotPackableAsync(in ProjectContext project, string disablePackingWarning, string notPresent)
    {
        if (StringComparer.OrdinalIgnoreCase.Equals(disablePackingWarning, notPresent))
        {
            this._logger.NonPackableProjectDoesNotDefineWarnOnPackingNonPackableProject(project.Name, PROPERTY_NAME);
        }
        else if (!StringComparer.OrdinalIgnoreCase.Equals(disablePackingWarning, "false"))
        {
            this._logger.NonPackableProjectDoesNotWarnOnPackingNonPackableProjectToFalse(project.Name, PROPERTY_NAME, disablePackingWarning);
        }

        return ValueTask.CompletedTask;
    }
}