using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TestHarnessExeProjectsMustSetTestingPlatformDotnetTestSupportToFalse
    : SimplePropertyProjectCheckBase
{
    public TestHarnessExeProjectsMustSetTestingPlatformDotnetTestSupportToFalse(
        ILogger<TestHarnessExeProjectsMustSetTestingPlatformDotnetTestSupportToFalse> logger
    )
        : base(propertyName: "TestingPlatformDotnetTestSupport", requiredValue: "false", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(x: project.GetOutputType(), y: "Exe");
    }
}
