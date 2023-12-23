using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

[SuppressMessage(category: "ReSharper", checkId: "UnusedType.Global", Justification = "Created by DI")]
public sealed class TargetFrameworkIsSetCorrectlyPolicy : IProjectCheck
{
    private readonly string? _expected;
    private readonly ILogger<TargetFrameworkIsSetCorrectlyPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public TargetFrameworkIsSetCorrectlyPolicy(IRepositorySettings repositorySettings, ILogger<TargetFrameworkIsSetCorrectlyPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
        this._expected = repositorySettings.DotnetTargetFramework;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._expected))
        {
            // no frameworks defined - allow any
            return ValueTask.CompletedTask;
        }

        if (this._repositorySettings.IsCodeAnalysisSolution && !project.IsTestProject(projectName: projectName, logger: this._logger))
        {
            // Code analysis project has specific requirements
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "TargetFramework", requiredValue: "netstandard2.0", logger: this._logger);

            return ValueTask.CompletedTask;
        }

        string sdk = project.GetSdk();

        if (string.IsNullOrWhiteSpace(sdk))
        {
            // no SDK don't know what do do.
            return ValueTask.CompletedTask;
        }

        if (!sdk.StartsWith(value: "Microsoft.NET.", comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            // not a dotnet SDK so don't process this
            return ValueTask.CompletedTask;
        }

        if (string.IsNullOrEmpty(this._expected))
        {
            // no frameworks defined - allow any
            return ValueTask.CompletedTask;
        }

        string[] frameworks = this._expected.Split(";");

        switch (frameworks.Length)
        {
            case 0: return ValueTask.CompletedTask;
            case 1:
                ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "TargetFramework", frameworks[0], logger: this._logger);

                return ValueTask.CompletedTask;
            default:
                ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "TargetFrameworks", requiredValue: this._expected, logger: this._logger);

                return ValueTask.CompletedTask;
        }
    }
}