using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the symbol package format is set appropriately.
/// </summary>
public sealed class SymbolPackageFormatPolicy : IProjectCheck
{
    private readonly ILogger<SymbolPackageFormatPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public SymbolPackageFormatPolicy(ILogger<SymbolPackageFormatPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName,
                                       project: project,
                                       nodePresence: @"SymbolPackageFormat",
                                       requiredValue: project.IsAnalyzerOrSourceGenerator()
                                           ? "symbols.nupkg"
                                           : "snupkg",
                                       logger: this._logger);
    }
}