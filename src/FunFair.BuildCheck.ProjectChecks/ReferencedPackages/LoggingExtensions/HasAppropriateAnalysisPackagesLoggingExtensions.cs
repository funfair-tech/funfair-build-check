using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class HasAppropriateAnalysisPackagesLoggingExtensions
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "{projectName}: Contains bad reference to packages.")]
    public static partial void ContainsBadReferenceToPackages(this ILogger logger, string projectName);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Does not reference {mustIncludePackageId} with a PrivateAssets=\"{privateAssets}\" attribute")]
    public static partial void DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(this ILogger logger, string projectName, string privateAssets, string mustIncludePackageId);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Found {detectPackageId} but did not find analyzer {mustIncludePackageId}.")]
    public static partial void DidNotFindMustIncludePackageForDetectedPackage(this ILogger logger, string projectName, string detectPackageId, string mustIncludePackageId);
}