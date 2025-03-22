using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NuGetAuditLevelPolicy : IProjectCheck
{
    private const string PRODUCTION_PROJECT_LEVEL = "low";
    private const string TEST_PROJECT_LEVEL = "high";
    private readonly ILogger<NuGetAuditLevelPolicy> _logger;

    public NuGetAuditLevelPolicy(ILogger<NuGetAuditLevelPolicy> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        string requiredLevel = project.IsTestProject(logger: this._logger)
            ? TEST_PROJECT_LEVEL
            : PRODUCTION_PROJECT_LEVEL;

        ProjectValueHelpers.CheckValue(
            project: project,
            nodePresence: "NuGetAuditLevel",
            requiredValue: requiredLevel,
            logger: this._logger
        );

        return ValueTask.CompletedTask;
    }
}
