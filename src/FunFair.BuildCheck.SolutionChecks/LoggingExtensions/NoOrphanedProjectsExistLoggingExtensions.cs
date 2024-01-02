using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks.LoggingExtensions;

internal static partial class NoOrphanedProjectsExistLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{solutionFileName}: Could not get base path")]
    public static partial void CouldNotGetBasePath(this ILogger<NoOrphanedProjectsExist> logger, string solutionFileName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{solutionFileName}: Project {projectFileName} is not in solution, but in work folder")]
    public static partial void ProjectIsNotInSolutionButInWorkFolder(this ILogger<NoOrphanedProjectsExist> logger, string solutionFileName, string projectFileName);
}