using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class WarnOnPackingNonPackableProjectMetadataLoggingExtensions
{
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable project defines {propertyName} as {propertyValue} the property should not be specified at all."
    )]
    public static partial void PackableProjectDefinesNotWarnOnPackingNonPackableProjectToFalse(
        this ILogger<WarnOnPackingNonPackableProjectMetadata> logger,
        string projectName,
        string propertyName,
        string propertyValue
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable project does not define {propertyName} as false."
    )]
    public static partial void NonPackableProjectDoesNotDefineWarnOnPackingNonPackableProject(
        this ILogger<WarnOnPackingNonPackableProjectMetadata> logger,
        string projectName,
        string propertyName
    );

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable project defines {propertyName} as {propertyValue}. It should be set to false."
    )]
    public static partial void NonPackableProjectDoesNotWarnOnPackingNonPackableProjectToFalse(
        this ILogger<WarnOnPackingNonPackableProjectMetadata> logger,
        string projectName,
        string propertyName,
        string propertyValue
    );
   
}