using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

[SuppressMessage(
    category: "FunFair.CodeAnalysis",
    checkId: "FFS0012: Classes should be static, sealed or abstract",
    Justification = "Direct instantiation and subclassing are both required for data-driven property checks."
)]
public class SimplePropertyProjectCheckBase : IProjectCheck
{
    private readonly Func<ProjectContext, bool>? _canCheckOverride;
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
        this._canCheckOverride = canCheck;
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
        return this._canCheckOverride?.Invoke(project) ?? true;
    }
}
