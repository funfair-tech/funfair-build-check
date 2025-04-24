using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Models;
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
        foreach (PackageReference package in project.ReferencedPackageElements(this._logger))
        {
            // check for private asset (if it's private the build won't fail for a pre-release package)
            string? privateAssets = package.GetAttributeOrElement("PrivateAssets");

            this.CheckReference(
                projectName: project.Name,
                privateAssets: privateAssets,
                package: package
            );
        }

        return ValueTask.CompletedTask;
    }

    private void CheckReference(
        string projectName,
        string? privateAssets,
        in PackageReference package
    )
    {
        if (
            !string.IsNullOrEmpty(privateAssets)
            && StringComparer.OrdinalIgnoreCase.Equals(x: privateAssets, y: PACKAGE_PRIVATE_ASSETS)
        )
        {
            return;
        }

        string? version = package.Version;

        if (string.IsNullOrWhiteSpace(version))
        {
            this._logger.CouldNotParseVersion(
                projectName: projectName,
                packageId: package.Id,
                version: version ?? "<null>"
            );
            return;
        }

        this._logger.FoundNuGetPackageAtVersion(
            projectName: projectName,
            packageId: package.Id,
            version: version
        );

        if (!NuGetVersion.TryParse(value: version, out NuGetVersion? nuGetVersion))
        {
            this._logger.CouldNotParseVersion(
                projectName: projectName,
                packageId: package.Id,
                version: version
            );

            return;
        }

        if (nuGetVersion.IsPrerelease && !this._configuration.PreReleaseBuild)
        {
            this._logger.UsesPreReleaseVersion(
                projectName: projectName,
                packageId: package.Id,
                version: version
            );
        }
    }
}
