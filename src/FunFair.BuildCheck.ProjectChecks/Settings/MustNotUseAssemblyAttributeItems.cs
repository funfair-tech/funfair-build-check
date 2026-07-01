using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotUseAssemblyAttributeItems : IProjectCheck
{
    private readonly ILogger<MustNotUseAssemblyAttributeItems> _logger;

    public MustNotUseAssemblyAttributeItems(ILogger<MustNotUseAssemblyAttributeItems> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(xpath: "/Project/ItemGroup/AssemblyAttribute");

        if (nodes is not null)
        {
            foreach (XmlElement node in nodes.OfType<XmlElement>())
            {
                string attributeName = node.GetAttribute(name: "Include");
                this._logger.UsesAssemblyAttributeItem(projectName: project.Name, attributeName: attributeName);
            }
        }

        return ValueTask.CompletedTask;
    }
}
