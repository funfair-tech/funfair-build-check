using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class HasAppropriateAnalysisPackages : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = "All";

    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected HasAppropriateAnalysisPackages(
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
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/PackageReference"
        );

        bool foundSourcePackage = false;
        bool foundAnalyzerPackage = false;

        if (nodes is not null)
        {
            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                this.CheckPackageReference(
                    projectName: project.Name,
                    reference: reference,
                    foundSourcePackage: ref foundSourcePackage,
                    foundAnalyzerPackage: ref foundAnalyzerPackage
                );
            }
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
        XmlElement reference,
        ref bool foundSourcePackage,
        ref bool foundAnalyzerPackage
    )
    {
        string packageName = reference.GetAttribute(name: "Include");

        if (string.IsNullOrWhiteSpace(packageName))
        {
            this._logger.ContainsBadReferenceToPackages(projectName);

            return;
        }

        if (
            StringComparer.InvariantCultureIgnoreCase.Equals(
                x: this._detectPackageId,
                y: packageName
            )
        )
        {
            foundSourcePackage = true;
        }

        if (
            StringComparer.InvariantCultureIgnoreCase.Equals(
                x: this._mustIncludePackageId,
                y: packageName
            )
        )
        {
            foundAnalyzerPackage = true;
            string assets = reference.GetAttribute(name: "PrivateAssets");

            if (
                string.IsNullOrWhiteSpace(assets)
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
