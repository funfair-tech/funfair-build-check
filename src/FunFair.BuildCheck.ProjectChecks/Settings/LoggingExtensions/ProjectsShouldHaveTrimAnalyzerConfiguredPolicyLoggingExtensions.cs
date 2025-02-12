using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class ProjectsShouldHaveTrimAnalyzerConfiguredPolicyLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Project does not define {property}.")]
    public static partial void ProjectShouldConfigureTrimAnalyzer(this ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger, string projectName, string property);
}
