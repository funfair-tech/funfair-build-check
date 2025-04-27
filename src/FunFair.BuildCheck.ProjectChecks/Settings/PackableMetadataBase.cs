using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public abstract class PackableMetadataBase : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _property;

    protected PackableMetadataBase(string property, ILogger logger)
    {
        this._property = property;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        if (!project.HasProperty(this._property))
        {
            this._logger.PackableProjectDoesNotDefineProperty(projectName: project.Name, property: this._property);
        }

        return ValueTask.CompletedTask;
    }
}
