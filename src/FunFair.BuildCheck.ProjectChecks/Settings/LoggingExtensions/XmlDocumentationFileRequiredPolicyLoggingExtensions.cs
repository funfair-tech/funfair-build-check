using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class XmlDocumentationFileRequiredPolicyLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Test projects should not have XML Documentation")]
    public static partial void TestProjectsShouldNotHaveXmlDocumentation(this ILogger<XmlDocumentationFileRequiredPolicy> logger, string projectName);
}