using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the neutral resources language attribute is set appropriately.
/// </summary>
public sealed class GenerateNeutralResourcesLanguageAttributePolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<GenerateNeutralResourcesLanguageAttributePolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public GenerateNeutralResourcesLanguageAttributePolicy(ILogger<GenerateNeutralResourcesLanguageAttributePolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"GenerateNeutralResourcesLanguageAttribute",
                                       requiredValue: EXPECTED,
                                       logger: this._logger);
    }
}