using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NoPreReleaseNuGetPackages : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = @"All";
    private readonly ICheckConfiguration _configuration;
    private readonly ILogger<NoPreReleaseNuGetPackages> _logger;

    public NoPreReleaseNuGetPackages(ICheckConfiguration configuration, ILogger<NoPreReleaseNuGetPackages> logger)
    {
        this._configuration = configuration;
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: @"Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                continue;
            }

            // check for private asset (if it's private the build won't fail for a pre-release package)
            string privateAssets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrEmpty(privateAssets))
            {
                if (reference.SelectSingleNode(xpath: "PrivateAssets") is XmlElement privateAssetsElement)
                {
                    privateAssets = privateAssetsElement.InnerText;
                }
            }

            if (!string.IsNullOrEmpty(privateAssets) && StringComparer.OrdinalIgnoreCase.Equals(x: privateAssets, y: PACKAGE_PRIVATE_ASSETS))
            {
                continue;
            }

            string version = reference.GetAttribute(name: @"Version");

            this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

            if (!NuGetVersion.TryParse(value: version, out NuGetVersion? nuGetVersion))
            {
                this._logger.LogError($"{projectName}: Package {packageName} could not parse version {version}.");

                continue;
            }

            if (nuGetVersion.IsPrerelease && !this._configuration.PreReleaseBuild)
            {
                this._logger.LogError($"{projectName}: Package {packageName} uses pre-release version {version}.");
            }
        }
    }
}