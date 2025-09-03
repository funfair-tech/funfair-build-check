using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class OnlyExesShouldBePublishablePolicy : IProjectCheck
{
    private readonly bool _isUnitTestBase;
    private readonly ILogger<OnlyExesShouldBePublishablePolicy> _logger;

    private readonly Func<bool, bool, bool, string, bool> _packablePolicy;
    private readonly IRepositorySettings _repositorySettings;

    public OnlyExesShouldBePublishablePolicy(IRepositorySettings repositorySettings, ILogger<OnlyExesShouldBePublishablePolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
        this._isUnitTestBase = repositorySettings.IsUnitTestBase;

        string packable = repositorySettings.DotnetPublishable ?? "NONE";

        if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "NONE"))
        {
            this._packablePolicy = (_, _, _, _) => false;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "ALL"))
        {
            this._packablePolicy = (_, _, isTestProject, _) => !isTestProject;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "EXE_TOOL"))
        {
            this._packablePolicy = (isDotNetTool, isExe, isTestProject, _) => (isExe || isDotNetTool) && !isTestProject;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "EXE"))
        {
            this._packablePolicy = (_, isExe, isTestProject, _) => isExe && !isTestProject;
        }
        else
        {
            ImmutableHashSet<string> projects = GetProjects(packable);

            this._packablePolicy = (_, _, isTestProject, projectName) => NotATestProjectButPackable(isTestProject: isTestProject, projects: projects, projectName: projectName);
        }
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._repositorySettings.DotnetPublishable))
        {
            return ValueTask.CompletedTask;
        }

        bool isTestProject = this.IsProjectATestProject(project);

        bool isDotNetTool = project.IsDotNetTool();

        bool isExe = StringComparer.OrdinalIgnoreCase.Equals(x: "Exe", project.GetOutputType());

        bool packable = this._packablePolicy(arg1: isDotNetTool, arg2: isExe, arg3: isTestProject, arg4: project.Name);

        ProjectValueHelpers.CheckValue(project: project, nodePresence: "IsPublishable", requiredValue: packable, logger: this._logger);

        return ValueTask.CompletedTask;
    }

    private static bool NotATestProjectButPackable(bool isTestProject, ImmutableHashSet<string> projects, string projectName)
    {
        return !isTestProject && projects.Contains(projectName);
    }

    private bool IsProjectATestProject(in ProjectContext project)
    {
        if (!project.IsTestProject(logger: this._logger))
        {
            return false;
        }

        return this.IsUnitTestBase(project) || !this._isUnitTestBase;
    }

    private bool IsUnitTestBase(in ProjectContext project)
    {
        return this._isUnitTestBase && project.Name.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
    }

    private static ImmutableHashSet<string> GetProjects(string packable)
    {
        return packable.Split(",")
                       .Select(static p => p.Trim())
                       .Where(static p => !string.IsNullOrWhiteSpace(p))
                       .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    }
}