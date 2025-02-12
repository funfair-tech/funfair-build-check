using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class NoPreReleaseNuGetPackagesLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "{projectName}: Found: {packageId} ({version})")]
    [Conditional("DEBUG")]
    public static partial void FoundNuGetPackageAtVersion(this ILogger<NoPreReleaseNuGetPackages> logger, string projectName, string packageId, string version);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{projectName}: Package {packageId} could not parse version {version}.")]
    public static partial void CouldNotParseVersion(this ILogger<NoPreReleaseNuGetPackages> logger, string projectName, string packageId, string version);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "{projectName}: Package {packageId} uses pre-release version {version}.")]
    public static partial void UsesPreReleaseVersion(this ILogger<NoPreReleaseNuGetPackages> logger, string projectName, string packageId, string version);
}
