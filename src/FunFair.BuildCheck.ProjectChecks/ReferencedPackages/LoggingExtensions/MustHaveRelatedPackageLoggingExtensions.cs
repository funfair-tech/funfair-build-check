using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class MustHaveRelatedPackageLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Found {detectPackageId} but did not find required {mustIncludePackageId}."
    )]
    public static partial void DidNotFindRelatedPackageForDetectedPackage(
        this ILogger logger,
        string projectName,
        string detectPackageId,
        string mustIncludePackageId
    );
}
