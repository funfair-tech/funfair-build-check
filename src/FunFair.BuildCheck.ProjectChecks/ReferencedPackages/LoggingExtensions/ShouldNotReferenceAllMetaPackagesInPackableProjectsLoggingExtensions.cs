using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;

internal static partial class ShouldNotReferenceAllMetaPackagesInPackableProjectsLoggingExtensions
{
    [LoggerMessage(EventId = 1,
                   Level = LogLevel.Error,
                   Message =
                       "{projectName}: References meta-package {packageId} rather than the individual packages -> It needs to use individual packages to control the nuget dependencies for users of the published package.")]
    public static partial void DoNotReferenceMetaPackageInPackableProjects(this ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger,
                                                                           string projectName,
                                                                           string packageId);
}