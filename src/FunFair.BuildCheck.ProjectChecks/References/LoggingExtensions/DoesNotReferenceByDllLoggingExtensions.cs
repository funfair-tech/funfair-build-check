using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;

internal static partial class DoesNotReferenceByDllLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: References {assembly} directly not using NuGet or a project reference."
    )]
    public static partial void ReferencesAssemblyDirectlyRatherThanThroughReference(
        this ILogger<DoesNotReferenceByDll> logger,
        string projectName,
        string assembly
    );
}
