using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NonTestProjectsMustNotDefineIsTestingPlatformApplication : MustNotDefinePropertyProjectCheckBase
{
    private readonly ILogger<NonTestProjectsMustNotDefineIsTestingPlatformApplication> _logger;

    public NonTestProjectsMustNotDefineIsTestingPlatformApplication(
        ILogger<NonTestProjectsMustNotDefineIsTestingPlatformApplication> logger
    )
        : base(propertyName: "IsTestingPlatformApplication", logger: logger)
    {
        this._logger = logger;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        return !project.IsTestProject(logger: this._logger);
    }
}
