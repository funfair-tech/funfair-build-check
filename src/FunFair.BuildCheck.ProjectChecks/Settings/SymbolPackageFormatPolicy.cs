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
        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        ProjectValueHelpers.CheckValue(
            project: project,
            nodePresence: "SymbolPackageFormat",
            project.IsAnalyzerOrSourceGenerator() ? "symbols.nupkg" : "snupkg",
            logger: this._logger
        );

        return ValueTask.CompletedTask;
    }
}
