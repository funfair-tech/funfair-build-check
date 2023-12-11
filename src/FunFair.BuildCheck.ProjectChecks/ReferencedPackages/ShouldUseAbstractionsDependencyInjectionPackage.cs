using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsDependencyInjectionPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsDependencyInjectionPackage(ILogger<ShouldUseAbstractionsDependencyInjectionPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.DependencyInjection", usePackageId: "Microsoft.Extensions.DependencyInjection.Abstractions", logger: logger)
    {
    }
}