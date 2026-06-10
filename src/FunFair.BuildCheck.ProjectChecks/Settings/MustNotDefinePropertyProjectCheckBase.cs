using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotDefinePropertyProjectCheckBase : IProjectCheck
{
    private readonly ILogger<MustNotDefinePropertyProjectCheckBase> _logger;
    private readonly string _propertyName;

    public MustNotDefinePropertyProjectCheckBase(
        string propertyName,
        ILogger<MustNotDefinePropertyProjectCheckBase> logger
    )
    {
        this._propertyName = propertyName;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        this.DoCheck(project: project);

        return ValueTask.CompletedTask;
    }

    private void DoCheck(in ProjectContext project)
    {
        if (project.IsPropertyDefined(this._propertyName))
        {
            this._logger.ProjectShouldNotDefineProperty(project.Name, this._propertyName);
        }
    }
}
