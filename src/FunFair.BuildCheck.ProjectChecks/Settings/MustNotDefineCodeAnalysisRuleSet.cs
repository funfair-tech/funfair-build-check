using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotDefineCodeAnalysisRuleSet : MustNotDefinePropertyProjectCheckBase
{
    public MustNotDefineCodeAnalysisRuleSet(ILogger<MustNotDefineCodeAnalysisRuleSet> logger)
        : base(propertyName: "CodeAnalysisRuleSet", logger: logger) { }
}
