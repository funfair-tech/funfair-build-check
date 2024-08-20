using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
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

    public ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        this._logger.CheckingSolution(solutionFileName);

        foreach (string projectFileName in this.GetMissingProjectFileNames())
        {
            this._logger.ProjectDoesNotExist(solutionFileName: solutionFileName, projectFileName: projectFileName);
        }

        return ValueTask.CompletedTask;
    }

    private IEnumerable<string> GetMissingProjectFileNames()
    {
        return this._projects.Select(static project => project.FileName)
                   .Where(static projectFileName => !File.Exists(PathHelpers.ConvertToNative(projectFileName)));
    }
}