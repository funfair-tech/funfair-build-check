using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NonTestProjectsMustNotDefineIsTestingPlatformApplication : MustNotDefinePropertyProjectCheckBase
{
    public NonTestProjectsMustNotDefineIsTestingPlatformApplication(
        ILogger<NonTestProjectsMustNotDefineIsTestingPlatformApplication> logger
    )
        : base(propertyName: "IsTestingPlatformApplication", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        string? value = project.GetProperty("IsTestProject");

        return value is not null && StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "false");
    }
}
