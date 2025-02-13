using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class PublishableExesMustHaveRuntimeIdentifiersLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Should specify RuntimeIdentifiers as it is publishable."
    )]
    public static partial void ShouldDefineRuntimeIdentifiers(
        this ILogger<PublishableExesMustHaveRuntimeIdentifiers> logger,
        string projectName
    );
}
