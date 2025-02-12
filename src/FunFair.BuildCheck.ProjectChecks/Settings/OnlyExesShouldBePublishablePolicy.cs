using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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

        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "NONE"))
        {
            this._packablePolicy = (_, _, _, _) => false;
        }
        else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "ALL"))
        {
            this._packablePolicy = (_, _, isTestProject, _) => !isTestProject;
        }
        else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "EXE_TOOL"))
        {
            this._packablePolicy = (isDotNetTool, isExe, isTestProject, _) => (isExe || isDotNetTool) && !isTestProject;
        }
        else if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packable, y: "EXE"))
        {
            this._packablePolicy = (_, isExe, isTestProject, _) => isExe && !isTestProject;
        }
        else
        {
            ImmutableHashSet<string> projects = GetProjects(packable);

            this._packablePolicy = (_, _, isTestProject, projectName) => !isTestProject && projects.Contains(projectName);
        }
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._repositorySettings.DotnetPublishable))
        {
            return ValueTask.CompletedTask;
        }

        bool isTestProject =
            project.IsTestProject(projectName: projectName, logger: this._logger)
            && (this._isUnitTestBase && projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase) || !this._isUnitTestBase);

        bool isDotNetTool = project.IsDotNetTool();

        bool isExe = StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", project.GetOutputType());

        bool packable = this._packablePolicy(arg1: isDotNetTool, arg2: isExe, arg3: isTestProject, arg4: projectName);

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "IsPublishable", requiredValue: packable, logger: this._logger);

        return ValueTask.CompletedTask;
    }

    private static ImmutableHashSet<string> GetProjects(string packable)
    {
        return packable.Split(",").Select(static p => p.Trim()).Where(static p => !string.IsNullOrWhiteSpace(p)).ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
