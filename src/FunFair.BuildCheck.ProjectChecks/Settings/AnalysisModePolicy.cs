using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class AnalysisModePolicy : IProjectCheck
{
    private const string EXPECTED = @"AllEnabledByDefault";

    private readonly ILogger<AnalysisModePolicy> _logger;

    public AnalysisModePolicy(ILogger<AnalysisModePolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"AnalysisMode", requiredValue: EXPECTED, logger: this._logger);
    }
}