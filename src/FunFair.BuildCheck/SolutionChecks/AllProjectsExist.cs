using System;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class AllProjectsExist : ISolutionCheck
    {
        private readonly ILogger<AllProjectsExist> _logger;

        public AllProjectsExist(ILogger<AllProjectsExist> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string solutionFileName)
        {
            this._logger.LogInformation($"Checking Solution: {solutionFileName}");
        }
    }
}