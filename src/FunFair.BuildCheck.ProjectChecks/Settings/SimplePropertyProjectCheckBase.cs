using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public abstract class SimplePropertyProjectCheckBase : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _propertyName;
    private readonly string _requiredValue;

    protected SimplePropertyProjectCheckBase(string propertyName, string requiredValue, ILogger logger)
    {
        this._propertyName = propertyName;
        this._requiredValue = requiredValue;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        this.DoCheck(project: project);

        return ValueTask.CompletedTask;
    }

    private void DoCheck(in ProjectContext project)
    {
        if (this.CanCheck(project: project))
        {
            ProjectValueHelpers.CheckValue(
                project: project,
                nodePresence: this._propertyName,
                requiredValue: this._requiredValue,
                logger: this._logger
            );
        }
    }

    protected virtual bool CanCheck(in ProjectContext project)
    {
        return true;
    }
}
