using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks
{
    /// <summary>
    ///     Checks to see if there are projects in the folder that aren't registered in the solution.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class NoOrphanedProjectsExist : ISolutionCheck
    {
        private readonly ILogger<AllProjectsExist> _logger;
        private readonly IReadOnlyList<Project> _projects;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="projects">The projects that are registered in the solution.</param>
        /// <param name="logger">Logging.</param>
        public NoOrphanedProjectsExist(IReadOnlyList<Project> projects, ILogger<AllProjectsExist> logger)
        {
            this._projects = projects ?? throw new ArgumentNullException(nameof(projects));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string solutionFileName)
        {
            string basePath = Path.GetDirectoryName(solutionFileName)!;
            IReadOnlyList<string> projectFileNames = Directory.EnumerateFiles(path: basePath, searchPattern: "*.csproj", searchOption: SearchOption.AllDirectories)
                                                              .OrderBy(x => x.ToLowerInvariant())
                                                              .ToArray();

            foreach (string project in projectFileNames)
            {
                if (this._projects.All(x => x.FileName != project))
                {
                    string solutionRelative = Path.GetRelativePath(relativeTo: basePath, path: project)!;
                    this._logger.LogError($"Project {solutionRelative} is not in solution, but in work folder");
                }
            }
        }
    }
}