using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class DoesNotReferenceByDll : IProjectCheck
{
    private readonly ILogger<DoesNotReferenceByDll> _logger;

    public DoesNotReferenceByDll(ILogger<DoesNotReferenceByDll> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        XmlNodeList? references = project.SelectNodes(xpath: "/Project/ItemGroup/Reference");

        if (references is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in references.OfType<XmlElement>())
        {
            string assembly = reference.GetAttribute(name: "Include");
            this._logger.ReferencesAssmbyDirectlyRatherThanThroughReference(projectName: projectName, packageId: assembly);
        }

        return ValueTask.CompletedTask;
    }
}