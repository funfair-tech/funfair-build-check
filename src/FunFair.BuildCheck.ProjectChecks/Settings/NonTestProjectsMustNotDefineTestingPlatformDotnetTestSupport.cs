using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport : MustNotDefinePropertyProjectCheckBase
{
    private readonly ILogger<NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport> _logger;

    public NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport(
        ILogger<NonTestProjectsMustNotDefineTestingPlatformDotnetTestSupport> logger
    )
        : base(propertyName: "TestingPlatformDotnetTestSupport", logger: logger)
    {
        this._logger = logger;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        return !project.IsTestProject(logger: this._logger);
    }
}
