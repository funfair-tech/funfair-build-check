using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotUseReferenceCoverlet : MustNotReferencePackages
{
    public MustNotUseReferenceCoverlet(ILogger<MustNotUseReferenceCoverlet> logger)
        : base(
            ["coverlet.collector", "coverlet.msbuild"],
            reason: "Built in to dotnet 8",
            logger: logger
        ) { }
}
