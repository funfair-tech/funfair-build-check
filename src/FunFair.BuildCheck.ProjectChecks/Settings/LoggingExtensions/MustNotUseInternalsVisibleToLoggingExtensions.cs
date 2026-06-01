using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class MustNotUseInternalsVisibleToLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Uses InternalsVisibleTo to expose internals to {assembly}"
    )]
    public static partial void UsesInternalsVisibleTo(
        this ILogger<MustNotUseInternalsVisibleTo> logger,
        string projectName,
        string assembly
    );
}
