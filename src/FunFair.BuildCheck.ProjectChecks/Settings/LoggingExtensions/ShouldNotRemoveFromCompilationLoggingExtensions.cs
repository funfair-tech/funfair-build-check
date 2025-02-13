using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class ShouldNotRemoveFromCompilationLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Removes {projectReference} from compilation"
    )]
    public static partial void RemovesProjectReferenceFromCompilation(
        this ILogger<ShouldNotRemoveFromCompilation> logger,
        string projectName,
        string projectReference
    );
}
