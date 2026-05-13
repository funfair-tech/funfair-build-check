using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class TestProjectsShouldImportUnitTestsPropsLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Test project should <Import Project=\"{projectImport}\" />"
    )]
    public static partial void TestProjectShouldImportUnitTestsProps(
        this ILogger<TestProjectsShouldImportUnitTestsProps> logger,
        string projectName,
        string projectImport
    );
}
