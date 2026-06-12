using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class SimplePropertyProjectCheckBase : IProjectCheck
{
    private readonly Func<ProjectContext, bool>? _canCheck;
    private readonly ILogger<SimplePropertyProjectCheckBase> _logger;
    private readonly string _propertyName;
    private readonly string _requiredValue;

    public SimplePropertyProjectCheckBase(
        string propertyName,
        string requiredValue,
        Func<ProjectContext, bool>? canCheck,
        ILogger<SimplePropertyProjectCheckBase> logger
    )
    {
        this._propertyName = propertyName;
        this._requiredValue = requiredValue;
        this._logger = logger;
        this._canCheck = canCheck;
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

    private bool CanCheck(in ProjectContext project)
    {
        return this._canCheck?.Invoke(project) ?? true;
    }
}
