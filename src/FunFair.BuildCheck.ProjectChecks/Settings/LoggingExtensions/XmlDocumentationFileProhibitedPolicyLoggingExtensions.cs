using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class XmlDocumentationFileProhibitedPolicyLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Should not have XML Documentation")]
    public static partial void ShouldNotHaveXmlDocumentation(this ILogger<XmlDocumentationFileProhibitedPolicy> logger, string projectName);
}
