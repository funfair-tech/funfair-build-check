using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsExtensionHostingPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsExtensionHostingPackage(ILogger<ShouldUseAbstractionsExtensionHostingPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.Hosting", usePackageId: "Microsoft.Extensions.Hosting.Abstractions", logger: logger)
    {
    }
}