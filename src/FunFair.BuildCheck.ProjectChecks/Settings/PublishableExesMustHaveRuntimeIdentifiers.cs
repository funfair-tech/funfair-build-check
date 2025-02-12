using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class PublishableExesMustHaveRuntimeIdentifiers : IProjectCheck
{
    private readonly ILogger<PublishableExesMustHaveRuntimeIdentifiers> _logger;

    public PublishableExesMustHaveRuntimeIdentifiers(ILogger<PublishableExesMustHaveRuntimeIdentifiers> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        bool isExe = StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", project.GetOutputType());

        if (!isExe)
        {
            return ValueTask.CompletedTask;
        }

        bool isPublishable = project.IsPublishable();

        if (!isPublishable)
        {
            return ValueTask.CompletedTask;
        }

        string runtimeIdentifiers = project.GetRuntimeIdentifiers();
        bool hasRuntimeIdentifiers = Array.Exists(runtimeIdentifiers.Split(";"), match: static item => !string.IsNullOrWhiteSpace(item));

        if (!hasRuntimeIdentifiers)
        {
            this._logger.ShouldDefineRuntimeIdentifiers(projectName);
        }

        return ValueTask.CompletedTask;
    }
}
