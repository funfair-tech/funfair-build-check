using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
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

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (project.SelectSingleNode(xpath: "/Project/ItemGroup/PackageReference[@Include='xunit']") is XmlElement)
        {
            // has an xunit reference so is a unit test project, don't force sourcelink
            return ValueTask.CompletedTask;
        }

        bool packageExists = CheckReference(packageId: PACKAGE_ID, project: project);
        bool historicalPackageExists = CheckReference(packageId: HISTORICAL_PACKAGE_ID, project: project);

        if (!packageExists && !historicalPackageExists)
        {
            this._logger.DoesNotReferencePackageOrHistoricalPackage(projectName: projectName, packageId: PACKAGE_ID, historicalPackageId: HISTORICAL_PACKAGE_ID);
        }

        if (packageExists && historicalPackageExists)
        {
            this._logger.ReferencesBothPackageAndHistoricalPackage(projectName: projectName, packageId: PACKAGE_ID, historicalPackageId: HISTORICAL_PACKAGE_ID);
        }

        if (packageExists)
        {
            if (!CheckPrivateAssets(packageId: PACKAGE_ID, project: project))
            {
                this._logger.DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(projectName: projectName, privateAssets: PACKAGE_ID, mustIncludePackageId: PACKAGE_PRIVATE_ASSETS);
            }
        }

        if (historicalPackageExists)
        {
            if (!CheckPrivateAssets(packageId: HISTORICAL_PACKAGE_ID, project: project))
            {
                this._logger.DoesNotReferenceMustIncludePackageIdWithAPrivateAssetsAttribute(
                    projectName: projectName,
                    privateAssets: HISTORICAL_PACKAGE_ID,
                    mustIncludePackageId: PACKAGE_PRIVATE_ASSETS
                );
            }
        }

        return ValueTask.CompletedTask;
    }

    private static bool CheckReference(string packageId, XmlDocument project)
    {
        XmlElement? reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;

        return reference is not null;
    }

    private static bool CheckPrivateAssets(string packageId, XmlDocument project)
    {
        if (project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") is not XmlElement reference)
        {
            return false;
        }

        // check for an attribute
        string assets = reference.GetAttribute(name: "PrivateAssets");

        if (string.IsNullOrEmpty(assets))
        {
            // no PrivateAssets attribute, check for an element
            if (reference.SelectSingleNode(xpath: "PrivateAssets") is not XmlElement privateAssets)
            {
                return false;
            }

            assets = privateAssets.InnerText;
        }

        return !string.IsNullOrEmpty(assets) && StringComparer.InvariantCultureIgnoreCase.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS);
    }
}
