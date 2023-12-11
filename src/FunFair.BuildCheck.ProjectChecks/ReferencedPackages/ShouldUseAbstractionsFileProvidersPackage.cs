using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsFileProvidersPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsFileProvidersPackage(ILogger<ShouldUseAbstractionsFileProvidersPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.FileProviders", usePackageId: "Microsoft.Extensions.FileProviders.Abstractions", logger: logger)
    {
    }
}