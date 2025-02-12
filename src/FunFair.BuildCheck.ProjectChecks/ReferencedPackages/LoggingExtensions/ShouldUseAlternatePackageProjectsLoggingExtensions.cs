using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class ShouldUseAlternatePackageProjectsLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Should use package {usePackageId} rather than {matchPackageId}")]
    public static partial void UseAlternatePackageIdForMatchedPackageId(this ILogger logger, string projectName, string usePackageId, string matchPackageId);
}
