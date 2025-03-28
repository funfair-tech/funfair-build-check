using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class GenerateSbomPolicy : SimplePropertyProjectCheckBase
{
    public GenerateSbomPolicy(ILogger<GenerateSbomPolicy> logger)
        : base(propertyName: "GenerateSBOM", requiredValue: "true", logger: logger) { }

    protected override bool CanCheck(in ProjectContext project)
    {
        return project.IsPackable();
    }
}
