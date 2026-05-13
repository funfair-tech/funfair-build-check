using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class TestProjectsMustImportUnitTestsPropsLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Test project should <Import Project=\"{projectImport}\" Condition=\"Exists('{projectImport}')\" />"
    )]
    public static partial void TestProjectShouldImportUnitTestsProps(
        this ILogger<TestProjectsMustImportUnitTestsProps> logger,
        string projectName,
        string projectImport
    );
}
