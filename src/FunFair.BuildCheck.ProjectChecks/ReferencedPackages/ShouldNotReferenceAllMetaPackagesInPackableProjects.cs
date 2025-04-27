using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldNotReferenceAllMetaPackagesInPackableProjects : IProjectCheck
{
    private readonly ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> _logger;

    public ShouldNotReferenceAllMetaPackagesInPackableProjects(
        ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger
    )
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        foreach (string packageId in project.ReferencedPackages(this._logger).Where(IsAllPackage))
        {
            this._logger.DoNotReferenceMetaPackageInPackableProjects(projectName: project.Name, packageId: packageId);
        }

        return ValueTask.CompletedTask;

        static bool IsAllPackage(string packageId)
        {
            return packageId.EndsWith(value: ".All", comparisonType: StringComparison.OrdinalIgnoreCase);
        }
    }
}
