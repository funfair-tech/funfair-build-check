using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class AllProjectsExist : ISolutionCheck
{
    private readonly ILogger<AllProjectsExist> _logger;
    private readonly IReadOnlyList<SolutionProject> _projects;

    public AllProjectsExist(IReadOnlyList<SolutionProject> projects, ILogger<AllProjectsExist> logger)
    {
        this._projects = projects;
        this._logger = logger;
    }

    public void Check(string solutionFileName)
    {
        this._logger.LogInformation($"Checking Solution: {solutionFileName}");

        foreach (string projectFileName in this._projects.Select(project => project.FileName)
                                               .Where(projectFileName => !File.Exists(PathHelpers.ConvertToNative(projectFileName))))
        {
            this._logger.LogError($"Project {projectFileName} does not exist");
        }
    }
}