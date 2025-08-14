using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class MustNotDefinePropertyProjectCheckBasePolicyLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Property {propertyName} is defined when it should not be."
    )]
    public static partial void ProjectShouldNotDefineProperty(
        this ILogger logger,
        string projectName,
        string propertyName
    );
}
