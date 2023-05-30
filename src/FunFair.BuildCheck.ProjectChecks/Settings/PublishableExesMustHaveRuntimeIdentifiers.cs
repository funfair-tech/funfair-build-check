using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableExesMustHaveRuntimeIdentifiers : IProjectCheck
{
    private readonly ILogger<PublishableExesMustHaveRuntimeIdentifiers> _logger;

    public PublishableExesMustHaveRuntimeIdentifiers(ILogger<PublishableExesMustHaveRuntimeIdentifiers> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        bool isExe = StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", project.GetOutputType());

        if (!isExe)
        {
            return;
        }

        bool isPublishable = project.IsPublishable();

        if (!isPublishable)
        {
            return;
        }

        string runtimeIdentifiers = project.GetRuntimeIdentifiers();
        bool hasRuntimeIdentifiers = Array.Exists(runtimeIdentifiers.Split(";"), match: item => !string.IsNullOrWhiteSpace(item));

        if (!hasRuntimeIdentifiers)
        {
            this._logger.LogError($"{projectName} Should specify RuntimeIdentifiers as it is publishable");
        }
    }
}