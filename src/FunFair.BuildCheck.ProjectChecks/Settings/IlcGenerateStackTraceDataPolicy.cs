using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IlcGenerateStackTraceDataPolicy : SimplePropertyProjectCheckBase
{
    public IlcGenerateStackTraceDataPolicy(ILogger<IlcGenerateStackTraceDataPolicy> logger)
        : base(propertyName: "IlcGenerateStackTraceData", requiredValue: "false", logger: logger) { }
}
