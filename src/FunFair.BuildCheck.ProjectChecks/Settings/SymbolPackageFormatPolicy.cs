using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class SymbolPackageFormatPolicy : IProjectCheck
{
    private readonly ILogger<SymbolPackageFormatPolicy> _logger;

    public SymbolPackageFormatPolicy(ILogger<SymbolPackageFormatPolicy> logger)
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
                                       nodePresence: @"SymbolPackageFormat",
                                       project.IsAnalyzerOrSourceGenerator()
                                           ? "symbols.nupkg"
                                           : "snupkg",
                                       logger: this._logger);
    }
}