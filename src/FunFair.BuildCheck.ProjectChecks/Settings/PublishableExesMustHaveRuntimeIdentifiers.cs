using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that any publishable exes specify RunTimeIdentifiers
/// </summary>
public sealed class PublishableExesMustHaveRuntimeIdentifiers : IProjectCheck
{
    private readonly ILogger<PublishableExesMustHaveRuntimeIdentifiers> _logger;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PublishableExesMustHaveRuntimeIdentifiers(ILogger<PublishableExesMustHaveRuntimeIdentifiers> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
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
        bool hasRuntimeIdentifiers = runtimeIdentifiers.Split(";")
                                                       .Any(item => !string.IsNullOrWhiteSpace(item));

        if (!hasRuntimeIdentifiers)
        {
            this._logger.LogError($"{projectName} Should specify RuntimeIdentifiers as it is publishable");
        }
    }
}