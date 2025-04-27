using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner.LoggingExtensions;

internal static partial class CheckRunnerLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Checking Project: {project}:")]
    public static partial void LogCheckingProject(this ITrackingLogger logger, string project);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Project: {projectFileName} could not get base path")]
    public static partial void LogCouldNotGetBasePathOfProject(this ITrackingLogger logger, string projectFileName);
}
