using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;

internal static partial class LibrariesShouldNotDependOnExecutablesLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{projectName}: Library references executable {referencedProject}.")]
    public static partial void LibraryReferencesExecutable(this ILogger<LibrariesShouldNotDependOnExecutables> logger, string projectName, string referencedProject);
}
