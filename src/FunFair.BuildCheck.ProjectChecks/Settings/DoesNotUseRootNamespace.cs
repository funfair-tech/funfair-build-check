using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class DoesNotUseRootNamespace : IProjectCheck
{
    private readonly ILogger<DoesNotUseRootNamespace> _logger;

    public DoesNotUseRootNamespace(ILogger<DoesNotUseRootNamespace> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (project.HasProperty("RootNamespace"))
        {
            this._logger.UsesRootNamespace(project.Name);
        }

        return ValueTask.CompletedTask;
    }
}
