using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class CodeAnalysisTreatWarningsAsErrorsPolicy : SimplePropertyProjectCheckBase
{
    public CodeAnalysisTreatWarningsAsErrorsPolicy(
        ILogger<CodeAnalysisTreatWarningsAsErrorsPolicy> logger
    )
        : base(
            propertyName: "CodeAnalysisTreatWarningsAsErrors",
            requiredValue: "true",
            logger: logger
        ) { }
}
