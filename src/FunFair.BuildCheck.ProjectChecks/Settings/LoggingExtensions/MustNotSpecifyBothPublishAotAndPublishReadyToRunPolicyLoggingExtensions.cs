using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicyLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Cannot specify both PublishAot and PublishReadyToRun as true."
    )]
    public static partial void CannotSpecifyBothPublishAotAndPublishReadyToRun(
        this ILogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger,
        string projectName
    );
}
