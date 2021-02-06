using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks
{
    /// <summary>
    ///     Checks to see if all projects in the solution exist.
    /// </summary>    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class AllProjectsExist : ISolutionCheck
    {
        private readonly ILogger<AllProjectsExist> _logger;
        private readonly IReadOnlyList<Project> _projects;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="projects">The projects that are registered in the solution.</param>
        /// <param name="logger">Logging.</param>
        public AllProjectsExist(IReadOnlyList<Project> projects, ILogger<AllProjectsExist> logger)
        {
            this._projects = projects ?? throw new ArgumentNullException(nameof(projects));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string solutionFileName)
        {
            this._logger.LogInformation($"Checking Solution: {solutionFileName}");

            foreach (Project? project in this._projects)
            {
                bool exists = File.Exists(project.FileName);

                if (!exists)
                {
                    this._logger.LogError($"Project {project.FileName} does not exist");
                }
            }
        }
    }
}
