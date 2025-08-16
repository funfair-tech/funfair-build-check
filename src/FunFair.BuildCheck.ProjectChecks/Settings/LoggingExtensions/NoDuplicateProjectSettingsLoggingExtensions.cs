using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class NoDuplicateProjectSettingsLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Property {propertyName} appears multiple times"
    )]
    public static partial void PropertyAppearsMultipleTimes(
        this ILogger<NoDuplicateProjectSettings> logger,
        string projectName,
        string propertyName
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{projectName}: Property {propertyName} appears multiple times in differing cases: {givenNames}"
    )]
    private static partial void PropertyAppearsInMultipleCases(
        this ILogger<NoDuplicateProjectSettings> logger,
        string projectName,
        string propertyName,
        string givenNames
    );

    public static void PropertyAppearsInMultipleCases(
        this ILogger<NoDuplicateProjectSettings> logger,
        string projectName,
        string propertyName,
        HashSet<string> givenNames
    )
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            logger.PropertyAppearsInMultipleCases(
                projectName: projectName,
                propertyName: propertyName,
                string.Join(separator: ", ", values: givenNames)
            );
        }
    }
}
