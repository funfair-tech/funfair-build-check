using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XUnitV3ProjectsShouldBeAnExecutable : SimplePropertyProjectCheckBase
{
    private readonly ILogger<XUnitV3ProjectsShouldBeAnExecutable> _logger;

    public XUnitV3ProjectsShouldBeAnExecutable(ILogger<XUnitV3ProjectsShouldBeAnExecutable> logger)
        : base(propertyName: "OutputType", requiredValue: "Exe", logger: logger)
    {
        this._logger = logger;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        if (
            project.IsTestProject(logger: this._logger)
            && project.ReferencesPackage("xunit.v3", this._logger)
        )
        {
            return true;
        }

        return false;
    }
}
