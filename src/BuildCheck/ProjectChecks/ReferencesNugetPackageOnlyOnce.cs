using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class ReferencesNugetPackageOnlyOnce : IProjectCheck
    {
        private const string PACKAGE_PRIVATE_ASSETS = @"All";
        private readonly ILogger<ReferencesNugetPackageOnlyOnce> _logger;

        public ReferencesNugetPackageOnlyOnce(ILogger<ReferencesNugetPackageOnlyOnce> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            HashSet<string> packageReferences = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            foreach (XmlElement? reference in nodes)
            {
                if (reference == null)
                {
                    continue;
                }

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
                    XmlElement? privateAssetsElement = reference.SelectSingleNode(xpath: "PrivateAssets") as XmlElement;

                    if (privateAssetsElement != null)
                    {
                        privateAssets = privateAssetsElement.InnerText;
                    }
                }

                if (!string.IsNullOrEmpty(privateAssets) && string.Compare(privateAssets, PACKAGE_PRIVATE_ASSETS, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    continue;
                }

                if (!packageReferences.Add(packageName))
                {
                    this._logger.LogError($"{projectName}: Already references package {packageName}.");
                }
            }
        }
    }
}