using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsCachingPackage : ShouldUseAlternatePackage
{
    public ShouldUseAbstractionsCachingPackage(ILogger<ShouldUseAbstractionsCachingPackage> logger)
        : base(
            matchPackageId: "Microsoft.Extensions.Caching.Memory",
            usePackageId: "Microsoft.Extensions.Caching.Abstractions",
            logger: logger
        ) { }

    protected override bool ShouldExclude(in ProjectContext project, ILogger logger)
    {
        return project.IsTestProject(logger: logger);
    }
}
