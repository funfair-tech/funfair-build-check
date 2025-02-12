using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsConfigurationPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsConfigurationPackage(ILogger<ShouldUseAbstractionsConfigurationPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.Configuration", usePackageId: "Microsoft.Extensions.Configuration.Abstractions", logger: logger) { }
}
