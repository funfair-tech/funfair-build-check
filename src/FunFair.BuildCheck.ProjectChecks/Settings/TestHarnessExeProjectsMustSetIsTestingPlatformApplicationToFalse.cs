using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TestHarnessExeProjectsMustSetIsTestingPlatformApplicationToFalse : SimplePropertyProjectCheckBase
{
    public TestHarnessExeProjectsMustSetIsTestingPlatformApplicationToFalse(
        ILogger<TestHarnessExeProjectsMustSetIsTestingPlatformApplicationToFalse> logger
    )
        : base(propertyName: "IsTestingPlatformApplication", requiredValue: "false", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(x: project.GetOutputType(), y: "Exe");
    }
}
