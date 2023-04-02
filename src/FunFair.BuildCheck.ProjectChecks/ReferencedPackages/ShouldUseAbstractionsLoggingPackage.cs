using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsLoggingPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsLoggingPackage(ILogger<ShouldUseAbstractionsLoggingPackage> logger)
        : base(matchPackageId: @"Microsoft.Extensions.Logging", usePackageId: @"Microsoft.Extensions.Logging.Abstractions", logger: logger)
    {
    }
}