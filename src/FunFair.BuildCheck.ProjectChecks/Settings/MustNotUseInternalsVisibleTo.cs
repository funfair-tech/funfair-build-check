using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotUseInternalsVisibleTo : IProjectCheck
{
    private readonly ILogger<MustNotUseInternalsVisibleTo> _logger;

    public MustNotUseInternalsVisibleTo(ILogger<MustNotUseInternalsVisibleTo> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(xpath: "/Project/ItemGroup/InternalsVisibleTo");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement node in nodes.OfType<XmlElement>())
        {
            string assembly = node.GetAttribute(name: "Include");
            this._logger.UsesInternalsVisibleTo(projectName: project.Name, assembly: assembly);
        }

        return ValueTask.CompletedTask;
    }
}
