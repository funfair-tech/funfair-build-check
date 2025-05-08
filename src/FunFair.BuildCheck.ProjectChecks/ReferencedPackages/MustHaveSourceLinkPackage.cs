using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Models;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveSourceLinkPackage : IProjectCheck
{
    private const string HISTORICAL_PACKAGE_ID = "SourceLink.Create.CommandLine";
    private const string PACKAGE_ID = "Microsoft.SourceLink.GitHub";
    private const string PACKAGE_PRIVATE_ASSETS = "All";

    private readonly ILogger<MustHaveSourceLinkPackage> _logger;

    public MustHaveSourceLinkPackage(ILogger<MustHaveSourceLinkPackage> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (project.ReferencesAnyOfPackages(["xunit", "xunit.v3"], logger: this._logger))
        {
            // has an xunit reference so is a unit test project, don't force sourcelink
            return ValueTask.CompletedTask;
        }

        bool packageExists = project.ReferencesPackage(packageName: PACKAGE_ID, logger: this._logger);
        bool historicalPackageExists = project.ReferencesPackage(packageName: HISTORICAL_PACKAGE_ID, logger: this._logger);

        if (!packageExists && !historicalPackageExists)
        {
            this._logger.DoesNotReferencePackageOrHistoricalPackage(projectName: project.Name, packageId: PACKAGE_ID, historicalPackageId: HISTORICAL_PACKAGE_ID);
        }

        if (packageExists && historicalPackageExists)
        {
            this._logger.ReferencesBothPackageAndHistoricalPackage(projectName: project.Name, packageId: PACKAGE_ID, historicalPackageId: HISTORICAL_PACKAGE_ID);
        }

        if (packageExists)
        {
            if (!this.CheckPrivateAssets(packageId: PACKAGE_ID, project: project))
            {
                this._logger.DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(projectName: project.Name, privateAssets: PACKAGE_ID, mustIncludePackageId: PACKAGE_PRIVATE_ASSETS);
            }
        }

        if (historicalPackageExists)
        {
            if (!this.CheckPrivateAssets(packageId: HISTORICAL_PACKAGE_ID, project: project))
            {
                this._logger.DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(projectName: project.Name,
                                                                                             privateAssets: HISTORICAL_PACKAGE_ID,
                                                                                             mustIncludePackageId: PACKAGE_PRIVATE_ASSETS);
            }
        }

        return ValueTask.CompletedTask;
    }

    private bool CheckPrivateAssets(string packageId, in ProjectContext project)
    {
        PackageReference? package = project.GetNamedReferencedPackage(packageId, this._logger);

        if (package is null)
        {
            return false;
        }

        string? assets = package.Value.GetAttributeOrElement("PrivateAssets");

        return !string.IsNullOrEmpty(assets) && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS);
    }
}