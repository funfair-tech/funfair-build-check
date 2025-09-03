using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class LibrariesShouldBePackablePolicy : IProjectCheck
{
    private readonly bool _isUnitTestBase;
    private readonly ILogger<LibrariesShouldBePackablePolicy> _logger;

    private readonly Func<bool, bool, bool, string, bool> _packablePolicy;
    private readonly IRepositorySettings _repositorySettings;

    public LibrariesShouldBePackablePolicy(IRepositorySettings repositorySettings, ILogger<LibrariesShouldBePackablePolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
        this._isUnitTestBase = repositorySettings.IsUnitTestBase;

        string packable = repositorySettings.DotnetPackable ?? "NONE";

        if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "NONE"))
        {
            this._packablePolicy = (_, _, _, _) => false;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "ALL"))
        {
            this._packablePolicy = (_, _, isTestProject, _) => !isTestProject;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "LIBRARIES"))
        {
            this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => isLibrary && !isTestProject && !isDotNetTool;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "LIBRARY_TOOL"))
        {
            this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => (isLibrary || isDotNetTool) && !isTestProject;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(x: packable, y: "TOOLS"))
        {
            this._packablePolicy = (isDotNetTool, isLibrary, isTestProject, _) => !isLibrary && !isTestProject && isDotNetTool;
        }
        else
        {
            ImmutableHashSet<string> projects = GetProjects(packable);

            this._packablePolicy = (_, _, isTestProject, projectName) => NotATestProjectButPackable(isTestProject: isTestProject, projects: projects, projectName: projectName);
        }
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._repositorySettings.DotnetPackable))
        {
            return ValueTask.CompletedTask;
        }

        bool isTestProject = project.IsTestProject(logger: this._logger) &&
                             ((this._isUnitTestBase && project.Name.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase)) || !this._isUnitTestBase);

        bool isDotNetTool = project.IsDotNetTool();

        bool isLibrary = StringComparer.OrdinalIgnoreCase.Equals(x: "Library", project.GetOutputType());

        bool packable = this._packablePolicy(arg1: isDotNetTool, arg2: isLibrary, arg3: isTestProject, arg4: project.Name);

        ProjectValueHelpers.CheckValue(project: project, nodePresence: "IsPackable", requiredValue: packable, logger: this._logger);

        return ValueTask.CompletedTask;
    }

    private static bool NotATestProjectButPackable(bool isTestProject, ImmutableHashSet<string> projects, string projectName)
    {
        return !isTestProject && projects.Contains(projectName);
    }

    private static ImmutableHashSet<string> GetProjects(string packable)
    {
        return packable.Split(",")
                       .Select(static p => p.Trim())
                       .Where(static p => !string.IsNullOrWhiteSpace(p))
                       .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    }
}