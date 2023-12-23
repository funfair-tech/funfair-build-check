using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustUseOpenApiAnalyzers : SimplePropertyProjectCheckBase
{
    public MustUseOpenApiAnalyzers(ILogger<MustUseOpenApiAnalyzers> logger)
        : base(propertyName: "IncludeOpenAPIAnalyzers", requiredValue: "true", logger: logger)
    {
    }
}