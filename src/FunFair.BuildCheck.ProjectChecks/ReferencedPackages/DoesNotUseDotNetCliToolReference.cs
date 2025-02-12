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

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/DotNetCliToolReference");

        if (nodes is not null && nodes.Count != 0)
        {
            this._logger.ContainsDotNetCliToolReference(projectName);
        }

        return ValueTask.CompletedTask;
    }
}
