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

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/DotNetCliToolReference");

        if (nodes != null && nodes.Count != 0)
        {
            this._logger.ContainsDotNetCliToolReference(projectName);
        }
    }
}