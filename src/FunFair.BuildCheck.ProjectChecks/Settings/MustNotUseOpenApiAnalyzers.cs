using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotUseOpenApiAnalyzers : MustNotDefinePropertyProjectCheckBase
{
    public MustNotUseOpenApiAnalyzers(ILogger<MustNotUseOpenApiAnalyzers> logger)
        : base(propertyName: "IncludeOpenAPIAnalyzers", logger: logger) { }
}