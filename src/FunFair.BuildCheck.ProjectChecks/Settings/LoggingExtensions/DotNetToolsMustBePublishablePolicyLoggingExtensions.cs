using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class DotNetToolsMustBePublishablePolicyLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: DotNet tools must have IsPublishable set to true so RID-specific tool packages include the entry point binary."
    )]
    public static partial void MustBePublishable(
        this ILogger<DotNetToolsMustBePublishablePolicy> logger,
        string projectName
    );
}
