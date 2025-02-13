using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class ReferencesNugetPackageOnlyOnceLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Already references package {packageId}."
    )]
    public static partial void AlreadyReferencesPackage(
        this ILogger<ReferencesNugetPackageOnlyOnce> logger,
        string projectName,
        string packageId
    );
}
