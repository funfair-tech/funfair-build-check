using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class DoesNotUseDotNetCliToolReferenceLoggingExtensions
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "{projectName}: Contains DotNetCliToolReference.")]
    public static partial void ContainsDotNetCliToolReference(this ILogger logger, string projectName);
}