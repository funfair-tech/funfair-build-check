using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class HasConsistentNuGetPackages : IProjectCheck
{
    private readonly ICheckConfiguration _checkConfiguration;
    private readonly ILogger<HasConsistentNuGetPackages> _logger;

    private readonly Dictionary<string, NuGetVersion> _packages;

    public HasConsistentNuGetPackages(
        ICheckConfiguration checkConfiguration,
        ILogger<HasConsistentNuGetPackages> logger
    )
    {
        this._checkConfiguration = checkConfiguration;
        this._logger = logger;

        this._packages = new(StringComparer.OrdinalIgnoreCase);
    }

    public ValueTask CheckAsync(
        string projectName,
        string projectFolder,
        XmlDocument project,
        CancellationToken cancellationToken
    )
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                this._logger.ContainsBadReferenceToPackages(projectName);

                continue;
            }

            string version = reference.GetAttribute(name: "Version");

            this._logger.FoundPackageVersion(
                projectName: projectName,
                packageName: packageName,
                version: version
            );

            this.CheckPackageReference(
                projectName: projectName,
                packageName: packageName,
                version: version
            );
        }

        return ValueTask.CompletedTask;
    }

    private void CheckPackageReference(string projectName, string packageName, string version)
    {
        if (!NuGetVersion.TryParse(value: version, out NuGetVersion? nuGetVersion))
        {
            this._logger.CouldNotParsePackageVersion(
                projectName: projectName,
                packageName: packageName,
                version: version
            );

            return;
        }

        string packageAsKey = packageName.ToLowerInvariant();

        if (!this._packages.TryGetValue(key: packageAsKey, out NuGetVersion? currentVersion))
        {
            this._packages.Add(key: packageAsKey, value: nuGetVersion);
        }
        else if (currentVersion != nuGetVersion)
        {
            this.LogVersionMismatch(
                projectName: projectName,
                packageName: packageName,
                nuGetVersion: nuGetVersion,
                currentVersion: currentVersion
            );

            return;
        }

        this._logger.UsingPackageAtVersion(
            projectName: projectName,
            packageName: packageName,
            installedVersion: nuGetVersion
        );
    }

    private void LogVersionMismatch(
        string projectName,
        string packageName,
        NuGetVersion nuGetVersion,
        NuGetVersion currentVersion
    )
    {
        if (this._checkConfiguration.AllowPackageVersionMismatch)
        {
            this._logger.UsingInconsistentPackageVersionInfo(
                projectName: projectName,
                packageName: packageName,
                installedVersion: nuGetVersion,
                currentVersion: currentVersion
            );
        }
        else
        {
            this._logger.UsingInconsistentPackageVersionError(
                projectName: projectName,
                packageName: packageName,
                installedVersion: nuGetVersion,
                currentVersion: currentVersion
            );
        }
    }
}
