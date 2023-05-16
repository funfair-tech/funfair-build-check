using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IncludeSymbolsPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<IncludeSymbolsPolicy> _logger;

    public IncludeSymbolsPolicy(ILogger<IncludeSymbolsPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IncludeSymbols", requiredValue: EXPECTED, logger: this._logger);
    }
}