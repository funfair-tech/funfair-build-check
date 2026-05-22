using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class PublishableExesMustHavePublishAotSetPolicyLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Should define PublishAot as true or false."
    )]
    public static partial void ShouldDefinePublishAot(
        this ILogger<PublishableExesMustHavePublishAotSetPolicy> logger,
        string projectName
    );
}
