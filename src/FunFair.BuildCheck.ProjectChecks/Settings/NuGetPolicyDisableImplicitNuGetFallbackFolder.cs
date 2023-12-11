using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NuGetPolicyDisableImplicitNuGetFallbackFolder : IProjectCheck
{
    private readonly ILogger<NuGetPolicyDisableImplicitNuGetFallbackFolder> _logger;

    public NuGetPolicyDisableImplicitNuGetFallbackFolder(ILogger<NuGetPolicyDisableImplicitNuGetFallbackFolder> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "DisableImplicitNuGetFallbackFolder", requiredValue: "true", logger: this._logger);
    }
}