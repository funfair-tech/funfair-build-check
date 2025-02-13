using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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

    public ValueTask CheckAsync(
        string projectName,
        string projectFolder,
        XmlDocument project,
        CancellationToken cancellationToken
    )
    {
        string requiredLevel = project.IsTestProject(projectName: projectName, logger: this._logger)
            ? TEST_PROJECT_LEVEL
            : PRODUCTION_PROJECT_LEVEL;

        ProjectValueHelpers.CheckValue(
            projectName: projectName,
            project: project,
            nodePresence: "NuGetAuditLevel",
            requiredValue: requiredLevel,
            logger: this._logger
        );

        return ValueTask.CompletedTask;
    }
}
