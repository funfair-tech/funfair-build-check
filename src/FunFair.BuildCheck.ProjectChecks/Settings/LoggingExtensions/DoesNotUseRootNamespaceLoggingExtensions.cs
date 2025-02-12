using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class DoesNotUseRootNamespaceLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Uses the RootNamespace setting to rename the namespace, when it should not")]
    public static partial void UsesRootNamespace(this ILogger<DoesNotUseRootNamespace> logger, string projectName);
}
