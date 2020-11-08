using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that a project only references a package once.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ReferencesNugetPackageOnlyOnce : IProjectCheck
    {
        private const string PACKAGE_PRIVATE_ASSETS = @"All";
        private readonly ILogger<ReferencesNugetPackageOnlyOnce> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ReferencesNugetPackageOnlyOnce(ILogger<ReferencesNugetPackageOnlyOnce> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
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
                    if (reference.SelectSingleNode(xpath: "PrivateAssets") is XmlElement privateAssetsElement)
                    {
                        privateAssets = privateAssetsElement.InnerText;
                    }
                }

                if (!string.IsNullOrEmpty(privateAssets) && StringComparer.OrdinalIgnoreCase.Equals(x: privateAssets, y: PACKAGE_PRIVATE_ASSETS))
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