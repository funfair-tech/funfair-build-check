using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the validate executable references match property is set appropriately.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class ValidateExecutableReferencesMatchSelfContainedPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<ValidateExecutableReferencesMatchSelfContainedPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ValidateExecutableReferencesMatchSelfContainedPolicy(ILogger<ValidateExecutableReferencesMatchSelfContainedPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
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