using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;

internal static partial class MustNotUseAssemblyAttributeItemsLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{projectName}: Uses <AssemblyAttribute Include=\"{attributeName}\" /> to inject an assembly attribute; assembly attributes must not be specified in the csproj"
    )]
    public static partial void UsesAssemblyAttributeItem(
        this ILogger<MustNotUseAssemblyAttributeItems> logger,
        string projectName,
        string attributeName
    );
}
