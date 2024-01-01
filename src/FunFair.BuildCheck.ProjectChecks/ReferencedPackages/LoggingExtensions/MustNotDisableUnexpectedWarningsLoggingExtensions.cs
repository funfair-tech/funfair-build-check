using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

public static partial class MustNotDisableUnexpectedWarningsLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Configuration {configuration} hides warning {warning}.")]
    public static partial void ConfigurationHidesWarning(this ILogger<MustNotDisableUnexpectedWarnings> logger, string projectName, string configuration, string warning);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Global Configuration hides warning {warning}.")]
    public static partial void GlobalConfigurationHidesWarning(this ILogger<MustNotDisableUnexpectedWarnings> logger, string projectName, string warning);
}