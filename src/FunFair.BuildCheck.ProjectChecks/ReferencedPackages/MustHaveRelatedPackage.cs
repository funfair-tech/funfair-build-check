using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class MustHaveRelatedPackage : IProjectCheck
{
    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected MustHaveRelatedPackage(string detectPackageId, string mustIncludePackageId, ILogger logger)
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        bool foundSourcePackage = false;
        bool foundRelatedPackage = false;

        foreach (string packageId in project.ReferencedPackages(this._logger))
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(x: this._detectPackageId, y: packageId))
            {
                foundSourcePackage = true;
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(x: this._mustIncludePackageId, y: packageId))
            {
                foundRelatedPackage = true;
            }
        }

        if (foundSourcePackage && !foundRelatedPackage)
        {
            this._logger.DidNotFindRelatedPackageForDetectedPackage(
                projectName: project.Name,
                detectPackageId: this._detectPackageId,
                mustIncludePackageId: this._mustIncludePackageId
            );
        }

        return ValueTask.CompletedTask;
    }
}
