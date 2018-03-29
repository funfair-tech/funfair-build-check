using System;
using Microsoft.Extensions.Logging;

namespace BuildCheck.SolutionChecks
{
    public class AllProjectsExist : ISolutionCheck
    {
        private readonly ILogger<AllProjectsExist> _logger;

        public AllProjectsExist(ILogger<AllProjectsExist> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Check(string solutionFileName)
        {
            this._logger.LogInformation($"Checking Solution: {solutionFileName}");
        }
    }
}