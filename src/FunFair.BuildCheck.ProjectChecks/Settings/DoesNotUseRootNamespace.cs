using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class DoesNotUseRootNamespace : IProjectCheck
{
    private readonly ILogger<DoesNotUseRootNamespace> _logger;

    public DoesNotUseRootNamespace(ILogger<DoesNotUseRootNamespace> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/RootNamespace");

        if (nodes is null)
        {
            return;
        }

        if (nodes.Count == 0)
        {
            return;
        }

        this._logger.LogError($"{projectName} Uses the RootNamepace setting to rename the namespace, when it should not");
    }
}