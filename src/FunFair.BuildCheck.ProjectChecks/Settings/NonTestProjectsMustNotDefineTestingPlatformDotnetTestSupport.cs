using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport : MustNotDefinePropertyProjectCheckBase
{
    public NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport(
        ILogger<NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport> logger
    )
        : base(propertyName: "TestingPlatformDotnetTestSupport", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        string? value = project.GetProperty("IsTestProject");

        return value is not null && StringComparer.OrdinalIgnoreCase.Equals(x: value, y: "false");
    }
}
