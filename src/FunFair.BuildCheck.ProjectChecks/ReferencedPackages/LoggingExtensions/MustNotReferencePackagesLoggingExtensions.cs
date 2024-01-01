using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

public static partial class MustNotReferencePackagesLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: References obsoleted package {packageId} using NuGet. {reason}")]
    public static partial void ReferencesObsoletedPackageUsingNuGet(this ILogger logger, string projectName, string packageId, string reason);
}