using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class PackableMetadataBaseLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable project does not define {property}."
    )]
    public static partial void PackableProjectDoesNotDefineProperty(
        this ILogger logger,
        string projectName,
        string property
    );

}