using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;

internal static partial class ReferencedProjectsMustExistLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Project references {referencedProject} that does not exist."
    )]
    public static partial void ReferencesProjectThatDoesNotExist(
        this ILogger<ReferencedProjectsMustExist> logger,
        string projectName,
        string referencedProject
    );
}
