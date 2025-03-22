using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
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
        XmlNodeList? nodes = project.CsProjXml.SelectNodes("/Project/PropertyGroup/RootNamespace");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        if (nodes.Count == 0)
        {
            return ValueTask.CompletedTask;
        }

        this._logger.UsesRootNamespace(project.Name);

        return ValueTask.CompletedTask;
    }
}
