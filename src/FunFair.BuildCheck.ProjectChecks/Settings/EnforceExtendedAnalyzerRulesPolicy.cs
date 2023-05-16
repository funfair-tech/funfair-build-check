using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnforceExtendedAnalyzerRulesPolicy : IProjectCheck
{
    private readonly ILogger<EnforceExtendedAnalyzerRulesPolicy> _logger;

    public EnforceExtendedAnalyzerRulesPolicy(ILogger<EnforceExtendedAnalyzerRulesPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"EnforceExtendedAnalyzerRules",
                                       project.IsAnalyzerOrSourceGenerator()
                                           ? "true"
                                           : "false",
                                       logger: this._logger);
    }
}