using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks.LoggingExtensions;

internal static partial class AllProjectsExistLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Checking Solution: {solutionFileName}"
    )]
    public static partial void CheckingSolution(
        this ILogger<AllProjectsExist> logger,
        string solutionFileName
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{solutionFileName}: Project {projectFileName} does not exist"
    )]
    public static partial void ProjectDoesNotExist(
        this ILogger<AllProjectsExist> logger,
        string solutionFileName,
        string projectFileName
    );
}
