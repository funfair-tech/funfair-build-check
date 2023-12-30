using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Helpers.LoggingExtensions;

internal static partial class ProjectValueHelpersLoggingExtensions
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "{projectName}: Contains bad reference to packages.")]
    public static partial void ContainsBadReferenceToPackages(this ILogger logger, string projectName);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Configuration {configuration} should specify {nodePresence}")]
    public static partial void ConfigurationShouldSpecifyNodePrescence(this ILogger logger, string projectName, string configuration, string nodePresence);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Configuration {configuration} should specify {nodePresence} as {requiredValueDisplayText}.")]
    public static partial void ConfigurationShouldSpecifyNodePrescenceAsValue(this ILogger logger, string projectName, string configuration, string nodePresence, string requiredValueDisplayText);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Should specify {nodePresence}")]
    public static partial void ProjectShouldSpecifyNodePrescence(this ILogger logger, string projectName, string nodePresence);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Should specify {nodePresence} as {requiredValueDisplayText}.")]
    public static partial void ProjectShouldSpecifyNodePrescenceAsValue(this ILogger logger, string projectName, string nodePresence, string requiredValueDisplayText);
}