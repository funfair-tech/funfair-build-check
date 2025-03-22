using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner
    : SimplePropertyProjectCheckBase
{
    private readonly ILogger<XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner> _logger;

    public XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner(
        ILogger<XUnitV3ProjectsShouldDefineUseMicrosoftTestingPlatformRunner> logger
    )
        : base(
            propertyName: "UseMicrosoftTestingPlatformRunner",
            requiredValue: "true",
            logger: logger
        )
    {
        this._logger = logger;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        if (project.IsTestProject(logger: this._logger))
        {
            if (
                project.ReferencesPackage("xunit.v3", this._logger)
                || project.ReferencesPackage("xunit.v3.extensibility.core", this._logger)
            )
            {
                return true;
            }
        }

        return false;
    }
}
