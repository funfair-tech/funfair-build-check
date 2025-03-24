using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnforceExtendedAnalyzerRulesPolicy : IProjectCheck
{
    private readonly ILogger<EnforceExtendedAnalyzerRulesPolicy> _logger;

    public EnforceExtendedAnalyzerRulesPolicy(ILogger<EnforceExtendedAnalyzerRulesPolicy> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        ProjectValueHelpers.CheckValue(
            project: project,
            nodePresence: "EnforceExtendedAnalyzerRules",
            project.IsAnalyzerOrSourceGenerator() ? "true" : "false",
            logger: this._logger
        );

        return ValueTask.CompletedTask;
    }
}
