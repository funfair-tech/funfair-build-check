using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Models;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class HasAppropriateAnalysisPackages : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = "All";

    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected HasAppropriateAnalysisPackages(string detectPackageId, string mustIncludePackageId, ILogger logger)
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        bool foundSourcePackage = false;
        bool foundAnalyzerPackage = false;

        foreach (PackageReference package in project.ReferencedPackageElements(this._logger))
        {
            this.CheckPackageReference(
                projectName: project.Name,
                package: package,
                foundSourcePackage: ref foundSourcePackage,
                foundAnalyzerPackage: ref foundAnalyzerPackage
            );
        }

        if (foundSourcePackage && !foundAnalyzerPackage)
        {
            this._logger.DidNotFindMustIncludePackageForDetectedPackage(
                projectName: project.Name,
                detectPackageId: this._detectPackageId,
                mustIncludePackageId: this._mustIncludePackageId
            );
        }

        return ValueTask.CompletedTask;
    }

    private void CheckPackageReference(
        string projectName,
        in PackageReference package,
        ref bool foundSourcePackage,
        ref bool foundAnalyzerPackage
    )
    {
        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._detectPackageId, y: package.Id))
        {
            foundSourcePackage = true;
        }

        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._mustIncludePackageId, y: package.Id))
        {
            foundAnalyzerPackage = true;

            if (
                !package.Attributes.TryGetValue(key: "PrivateAssets", out string? assets)
                || string.IsNullOrWhiteSpace(assets)
                || !StringComparer.Ordinal.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS)
            )
            {
                this._logger.DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(
                    projectName: projectName,
                    privateAssets: PACKAGE_PRIVATE_ASSETS,
                    mustIncludePackageId: this._mustIncludePackageId
                );
            }
        }
    }
}
