using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class GenerateNeutralResourcesLanguageAttributePolicy : IProjectCheck
{
    private const string EXPECTED = "true";

    private readonly ILogger<GenerateNeutralResourcesLanguageAttributePolicy> _logger;

    public GenerateNeutralResourcesLanguageAttributePolicy(ILogger<GenerateNeutralResourcesLanguageAttributePolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "GenerateNeutralResourcesLanguageAttribute", requiredValue: EXPECTED, logger: this._logger);
    }
}