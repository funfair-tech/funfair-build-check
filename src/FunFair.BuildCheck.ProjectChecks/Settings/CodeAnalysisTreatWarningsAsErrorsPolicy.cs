using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that Code Analysis warnings are set as errors.
/// </summary>
public sealed class CodeAnalysisTreatWarningsAsErrorsPolicy : IProjectCheck
{
    private const string EXPECTED = @"true";

    private readonly ILogger<CodeAnalysisTreatWarningsAsErrorsPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public CodeAnalysisTreatWarningsAsErrorsPolicy(ILogger<CodeAnalysisTreatWarningsAsErrorsPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"CodeAnalysisTreatWarningsAsErrors", requiredValue: EXPECTED, logger: this._logger);
    }
}