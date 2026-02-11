using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class SymbolPackageFormatPolicy : IProjectCheck
{
    private readonly ILogger<SymbolPackageFormatPolicy> _logger;

    public SymbolPackageFormatPolicy(ILogger<SymbolPackageFormatPolicy> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (CanCheck(project: project))
        {
            ProjectValueHelpers.CheckValue(
            project: project,
            nodePresence: "SymbolPackageFormat",
            requiredValue: "snupkg",
            logger: this._logger
        );
        }

        return ValueTask.CompletedTask;
    }

    private static bool CanCheck(in ProjectContext project)
    {
        return project.IsPackable() && !project.IsAnalyzerOrSourceGenerator() && !project.IsDotNetTool();
    }
}
