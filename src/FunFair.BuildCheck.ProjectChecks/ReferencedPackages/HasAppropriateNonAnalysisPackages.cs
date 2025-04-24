using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class HasAppropriateNonAnalysisPackages : IProjectCheck
{
    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected HasAppropriateNonAnalysisPackages(
        string detectPackageId,
        string mustIncludePackageId,
        ILogger logger
    )
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {

        bool foundSourcePackage = false;
        bool foundRequiredPackage = false;


        foreach (string packageId in project.ReferencedPackageElements(this._logger).Select(package => package.Id))
        {
            if (
                StringComparer.InvariantCultureIgnoreCase.Equals(
                    x: this._detectPackageId,
                    y: packageId
                )
            )
            {
                foundSourcePackage = true;
            }

            if (
                StringComparer.InvariantCultureIgnoreCase.Equals(
                    x: this._mustIncludePackageId,
                    y: packageId
                )
            )
            {
                foundRequiredPackage = true;
            }
        }

        if (foundSourcePackage && !foundRequiredPackage)
        {
            this._logger.DidNotFindMustIncludePackageForDetectedPackage(
                projectName: project.Name,
                detectPackageId: this._detectPackageId,
                mustIncludePackageId: this._mustIncludePackageId
            );
        }

        return ValueTask.CompletedTask;
    }
}
