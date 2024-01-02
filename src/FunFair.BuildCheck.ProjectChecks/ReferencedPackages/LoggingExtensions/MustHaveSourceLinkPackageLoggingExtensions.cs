using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class MustHaveSourceLinkPackageLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Does not reference {packageId} or {historicalPackageId} using NuGet")]
    public static partial void DoesNotReferencePackageOrHistoricalPackage(this ILogger<MustHaveSourceLinkPackage> logger, string projectName, string packageId, string historicalPackageId);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: References both {packageId} and {historicalPackageId}")]
    public static partial void ReferencesBothPackageAndHistoricalPackage(this ILogger<MustHaveSourceLinkPackage> logger, string projectName, string packageId, string historicalPackageId);
}