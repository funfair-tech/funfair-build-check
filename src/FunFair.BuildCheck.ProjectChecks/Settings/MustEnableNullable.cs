using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustEnableNullable : IProjectCheck
{
    private readonly ILogger<MustEnableNullable> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public MustEnableNullable(IRepositorySettings repositorySettings, ILogger<MustEnableNullable> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!this._repositorySettings.IsNullableGloballyEnforced)
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"Nullable", requiredValue: "enable", logger: this._logger);
    }
}