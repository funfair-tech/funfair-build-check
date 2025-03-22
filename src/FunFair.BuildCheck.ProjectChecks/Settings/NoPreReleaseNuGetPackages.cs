using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NoPreReleaseNuGetPackages : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = "All";
    private readonly ICheckConfiguration _configuration;
    private readonly ILogger<NoPreReleaseNuGetPackages> _logger;

    public NoPreReleaseNuGetPackages(
        ICheckConfiguration configuration,
        ILogger<NoPreReleaseNuGetPackages> logger
    )
    {
        this._configuration = configuration;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/PackageReference"
        );

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                this._logger.ContainsBadReferenceToPackages(project.Name);

                continue;
            }

            // check for private asset (if it's private the build won't fail for a pre-release package)
            string privateAssets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrEmpty(privateAssets))
            {
                if (
                    reference.SelectSingleNode(xpath: "PrivateAssets")
                    is XmlElement privateAssetsElement
                )
                {
                    privateAssets = privateAssetsElement.InnerText;
                }
            }

            this.CheckReference(
                projectName: project.Name,
                privateAssets: privateAssets,
                reference: reference,
                packageName: packageName
            );
        }

        return ValueTask.CompletedTask;
    }

    private void CheckReference(
        string projectName,
        string privateAssets,
        XmlElement reference,
        string packageName
    )
    {
        if (
            !string.IsNullOrEmpty(privateAssets)
            && StringComparer.OrdinalIgnoreCase.Equals(x: privateAssets, y: PACKAGE_PRIVATE_ASSETS)
        )
        {
            return;
        }

        string version = reference.GetAttribute(name: "Version");

        this._logger.FoundNuGetPackageAtVersion(
            projectName: projectName,
            packageId: packageName,
            version: version
        );

        if (!NuGetVersion.TryParse(value: version, out NuGetVersion? nuGetVersion))
        {
            this._logger.CouldNotParseVersion(
                projectName: projectName,
                packageId: packageName,
                version: version
            );

            return;
        }

        if (nuGetVersion.IsPrerelease && !this._configuration.PreReleaseBuild)
        {
            this._logger.UsesPreReleaseVersion(
                projectName: projectName,
                packageId: packageName,
                version: version
            );
        }
    }
}
