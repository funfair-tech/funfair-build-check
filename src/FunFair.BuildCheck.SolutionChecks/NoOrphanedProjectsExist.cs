using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class NoOrphanedProjectsExist : ISolutionCheck
{
    private readonly ILogger<NoOrphanedProjectsExist> _logger;
    private readonly IReadOnlyList<SolutionProject> _projects;

    public NoOrphanedProjectsExist(IReadOnlyList<SolutionProject> projects, ILogger<NoOrphanedProjectsExist> logger)
    {
        this._projects = projects;
        this._logger = logger;
    }

    public ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        string? basePath = Path.GetDirectoryName(solutionFileName);

        if (string.IsNullOrEmpty(basePath))
        {
            this._logger.CouldNotGetBasePath(solutionFileName);

            return ValueTask.CompletedTask;
        }

        this.CheckProjects(solutionFileName: solutionFileName, basePath: basePath);

        return ValueTask.CompletedTask;
    }

    private void CheckProjects(string solutionFileName, string basePath)
    {
        IReadOnlyList<string> projectFileNames = GetOrderedProjects(basePath);

        foreach (string project in projectFileNames.Where(this.ProjectIsInSolution))
        {
            this.CheckProject(solutionFileName: solutionFileName, basePath: basePath, project: project);
        }
    }

    private void CheckProject(string solutionFileName, string basePath, string project)
    {
        string solutionRelative = GetPathRelativeToSolution(basePath: basePath, project: project);
        this._logger.ProjectIsNotInSolutionButInWorkFolder(solutionFileName: solutionFileName, projectFileName: solutionRelative);
    }

    private static string GetPathRelativeToSolution(string basePath, string project)
    {
        return Path.GetRelativePath(relativeTo: basePath, path: project);
    }

    private bool ProjectIsInSolution(string project)
    {
        return this._projects.All(x => !StringComparer.Ordinal.Equals(x: x.FileName, y: project));
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