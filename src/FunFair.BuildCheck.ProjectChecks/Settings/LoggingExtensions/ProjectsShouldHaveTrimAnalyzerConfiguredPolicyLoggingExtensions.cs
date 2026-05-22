using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class ProjectsShouldHaveTrimAnalyzerConfiguredPolicyLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Project does not define {property}.")]
    public static partial void ProjectShouldConfigureTrimAnalyzer(
        this ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger,
        string projectName,
        string property
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{projectName}: {property} must be true when IsTrimmable is true."
    )]
    public static partial void ProjectMustEnableTrimAnalyzerWhenTrimmable(
        this ILogger<ProjectsShouldHaveTrimAnalyzerConfiguredPolicy> logger,
        string projectName,
        string property
    );
}
