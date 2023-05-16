using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class DoesNotReferenceByDll : IProjectCheck
{
    private readonly ILogger<DoesNotReferenceByDll> _logger;

    public DoesNotReferenceByDll(ILogger<DoesNotReferenceByDll> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? references = project.SelectNodes(xpath: "/Project/ItemGroup/Reference");

        if (references == null)
        {
            return;
        }

        foreach (XmlElement reference in references.OfType<XmlElement>())
        {
            string assembly = reference.GetAttribute(name: "Include");
            this._logger.LogError($"{projectName}: References {assembly} directly not using NuGet or a project reference.");
        }
    }
}