using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustEnableNullable : SimplePropertyProjectCheckBase
{
    private readonly IRepositorySettings _repositorySettings;

    public MustEnableNullable(IRepositorySettings repositorySettings, ILogger<MustEnableNullable> logger)
        : base(propertyName: "Nullable", requiredValue: "enable", logger: logger)
    {
        this._repositorySettings = repositorySettings;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        return this._repositorySettings.IsNullableGloballyEnforced;
    }
}
