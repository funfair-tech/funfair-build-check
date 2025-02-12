using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustEnableStrictMode : SimplePropertyProjectCheckBase
{
    public MustEnableStrictMode(ILogger<MustEnableStrictMode> logger)
        : base(propertyName: "Features", requiredValue: "strict;flow-analysis", logger: logger) { }
}
