using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class DoesNotUseDotNetCliToolReference : IProjectCheck
{
    private readonly ILogger<DoesNotUseDotNetCliToolReference> _logger;

    public DoesNotUseDotNetCliToolReference(ILogger<DoesNotUseDotNetCliToolReference> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/DotNetCliToolReference"
        );

        if (nodes is not null && nodes.Count != 0)
        {
            this._logger.ContainsDotNetCliToolReference(project.Name);
        }

        return ValueTask.CompletedTask;
    }
}
