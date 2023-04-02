using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustUseOpenApiAnalyzers : IProjectCheck
{
    private readonly ILogger<MustUseOpenApiAnalyzers> _logger;

    public MustUseOpenApiAnalyzers(ILogger<MustUseOpenApiAnalyzers> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IncludeOpenAPIAnalyzers", requiredValue: "true", logger: this._logger);
    }
}