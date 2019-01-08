using System;
using System.Xml;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace BuildCheck.ProjectChecks
{
    public class NoPreReleaseNuGetPackages : IProjectCheck
    {
        private readonly ICheckConfiguration _configuration;
        private readonly ILogger<NoPreReleaseNuGetPackages> _logger;
        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        public NoPreReleaseNuGetPackages(ICheckConfiguration configuration, ILogger<NoPreReleaseNuGetPackages> logger)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            foreach (XmlElement reference in nodes)
            {
                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                // check for private asset (if it's private the build won't fail for a pre-release package)
                string privateAssets = reference.GetAttribute("PrivateAssets");
                if (string.IsNullOrEmpty(privateAssets))
                {
                    XmlElement privateAssetsElement = reference.SelectSingleNode("PrivateAssets") as XmlElement;
                    if (privateAssetsElement != null)
                    {
                        privateAssets = privateAssetsElement.InnerText;
                    }
                }

                if (!string.IsNullOrEmpty(privateAssets) && string.Compare(privateAssets, PACKAGE_PRIVATE_ASSETS, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    continue;
                }

                string version = reference.GetAttribute(name: @"Version");

                this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

                if (!NuGetVersion.TryParse(version, out NuGetVersion nuGetVersion))
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
}