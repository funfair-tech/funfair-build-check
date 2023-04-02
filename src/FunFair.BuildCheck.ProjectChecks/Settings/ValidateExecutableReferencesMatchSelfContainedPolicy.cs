using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ValidateExecutableReferencesMatchSelfContainedPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<ValidateExecutableReferencesMatchSelfContainedPolicy> _logger;

    public ValidateExecutableReferencesMatchSelfContainedPolicy(ILogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPublishable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"ValidateExecutableReferencesMatchSelfContained",
                                       requiredValue: EXPECTED,
                                       logger: this._logger);
    }
}