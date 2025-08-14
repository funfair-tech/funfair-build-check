using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public abstract class MustNotDefinePropertyProjectCheckBase : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _propertyName;

    protected MustNotDefinePropertyProjectCheckBase(string propertyName, ILogger logger)
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
        if (!this.CanCheck(project: project))
        {
            return;
        }

        if (project.HasProperty(this._propertyName))
        {
            this._logger.ProjectShouldNotDefineProperty(project.Name, this._propertyName);
        }
    }

    protected virtual bool CanCheck(in ProjectContext project)
    {
        return true;
    }
}
