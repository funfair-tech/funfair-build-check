using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseNSubstituteRatherThanMoqPackage : ShouldUseAlternatePackage
{
    public ShouldUseNSubstituteRatherThanMoqPackage(
        ILogger<ShouldUseNSubstituteRatherThanMoqPackage> logger
    )
        : base(matchPackageId: "Moq", usePackageId: "NSubstitute", logger: logger) { }
}
