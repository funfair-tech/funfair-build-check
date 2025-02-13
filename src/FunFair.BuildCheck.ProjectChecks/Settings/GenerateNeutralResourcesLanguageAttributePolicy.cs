using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class GenerateNeutralResourcesLanguageAttributePolicy : SimplePropertyProjectCheckBase
{
    public GenerateNeutralResourcesLanguageAttributePolicy(
        ILogger<GenerateNeutralResourcesLanguageAttributePolicy> logger
    )
        : base(
            propertyName: "GenerateNeutralResourcesLanguageAttribute",
            requiredValue: "true",
            logger: logger
        ) { }
}
