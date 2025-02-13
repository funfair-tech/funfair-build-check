using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class HasConsistentNuGetPackagesLoggingExtensions
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Debug,
        Message = "{projectName}: Found: {packageName} ({version})"
    )]
    [Conditional("DEBUG")]
    public static partial void FoundPackageVersion(
        this ILogger<HasConsistentNuGetPackages> logger,
        string projectName,
        string packageName,
        string version
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Package {packageName} could not parse version {version}."
    )]
    public static partial void CouldNotParsePackageVersion(
        this ILogger<HasConsistentNuGetPackages> logger,
        string projectName,
        string packageName,
        string version
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{projectName}: Uses {packageName} version {installedVersion} when it should be using previously seen {currentVersion}."
    )]
    public static partial void UsingInconsistentPackageVersionError(
        this ILogger<HasConsistentNuGetPackages> logger,
        string projectName,
        string packageName,
        NuGetVersion installedVersion,
        NuGetVersion currentVersion
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "{projectName}: Uses {packageName} version {installedVersion} when it should be using previously seen {currentVersion}."
    )]
    public static partial void UsingInconsistentPackageVersionInfo(
        this ILogger<HasConsistentNuGetPackages> logger,
        string projectName,
        string packageName,
        NuGetVersion installedVersion,
        NuGetVersion currentVersion
    );

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Information,
        Message = "{projectName}: Uses {packageName} version {installedVersion}."
    )]
    public static partial void UsingPackageAtVersion(
        this ILogger<HasConsistentNuGetPackages> logger,
        string projectName,
        string packageName,
        NuGetVersion installedVersion
    );
}
