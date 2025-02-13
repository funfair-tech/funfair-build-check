using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class ImportCommonPropsLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable project should <Import Project=\"{projectImport}\" />"
    )]
    public static partial void PackableProjectShouldImportProject(
        this ILogger<ImportCommonProps> logger,
        string projectName,
        string projectImport
    );
}
