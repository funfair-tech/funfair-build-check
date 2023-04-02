using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class NoOrphanedProjectsExist : ISolutionCheck
{
    private readonly ILogger<NoOrphanedProjectsExist> _logger;
    private readonly IReadOnlyList<SolutionProject> _projects;

    public NoOrphanedProjectsExist(IReadOnlyList<SolutionProject> projects, ILogger<NoOrphanedProjectsExist> logger)
    {
        this._projects = projects ?? throw new ArgumentNullException(nameof(projects));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string solutionFileName)
    {
        string basePath = Path.GetDirectoryName(solutionFileName)!;
        IReadOnlyList<string> projectFileNames = GetOrderedProjects(basePath);

        foreach (string project in projectFileNames.Where(this.ProjectIsInSolution))
        {
            string solutionRelative = GetPathRelativeToSolution(basePath: basePath, project: project);
            this._logger.LogError($"Project {solutionRelative} is not in solution, but in work folder");
        }
    }

    private static string GetPathRelativeToSolution(string basePath, string project)
    {
        return Path.GetRelativePath(relativeTo: basePath, path: project);
    }

    private bool ProjectIsInSolution(string project)
    {
        return this._projects.All(x => x.FileName != project);
    }

    private static IReadOnlyList<string> GetOrderedProjects(string basePath)
    {
        return GetProjects(basePath)
               .OrderBy(keySelector: Ordering, comparer: StringComparer.Ordinal)
               .ToArray();
    }

    private static IEnumerable<string> GetProjects(string basePath)
    {
        return Directory.EnumerateFiles(path: basePath, searchPattern: "*.csproj", searchOption: SearchOption.AllDirectories);
    }

    private static string Ordering(string name)
    {
        return name.ToLowerInvariant();
    }
}