using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> _logger;

    public EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy(ILogger<EnableMicrosoftExtensionsConfigurationBinderSourceGeneratorPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
                                       requiredValue: EXPECTED,
                                       logger: this._logger);
    }
}