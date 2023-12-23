using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class LanguagePolicyUseLatestVersion : SimplePropertyProjectCheckBase
{
    public LanguagePolicyUseLatestVersion(ILogger<LanguagePolicyUseLatestVersion> logger)
        : base(propertyName: "LangVersion", requiredValue: "latest", logger: logger)
    {
    }
}