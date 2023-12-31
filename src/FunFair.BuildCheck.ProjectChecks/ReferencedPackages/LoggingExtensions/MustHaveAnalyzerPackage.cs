using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

public static partial class MustHaveAnalyzerPackage
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Does not reference {this._packageId} with a PrivateAssets=\"{privateAssets}\" attribute")]
    public static partial void DoesNotUsePrivateAssetsAttribute(this ILogger logger, string projectName, string packageName, string privateAssets);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Does not reference {this._packageId}  using NuGet")]
    public static partial void DoesNotUseNuGet(this ILogger logger, string projectName, string packageName);
}