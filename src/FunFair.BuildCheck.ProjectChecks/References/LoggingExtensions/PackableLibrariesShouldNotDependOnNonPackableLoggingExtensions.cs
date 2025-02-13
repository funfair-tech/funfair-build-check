using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;

internal static partial class PackableLibrariesShouldNotDependOnNonPackableLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Packable Library project references non-packable project {referencedProject}"
    )]
    public static partial void PackableProjectReferencesNonPackableProject(
        this ILogger<PackableLibrariesShouldNotDependOnNonPackable> logger,
        string projectName,
        string referencedProject
    );
}
