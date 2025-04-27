using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class MustHaveAnalyzerPackageLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Does not reference {packageId} with a PrivateAssets=\"{privateAssets}\" attribute"
    )]
    public static partial void DoesNotUsePrivateAssetsAttribute(
        this ILogger logger,
        string projectName,
        string packageId,
        string privateAssets
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{projectName}: Does not reference {packageId} using NuGet"
    )]
    public static partial void DoesNotUseNuGet(this ILogger logger, string projectName, string packageId);
}
