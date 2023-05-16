using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ReferencesNugetPackageOnlyOnce : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = @"All";
    private readonly ILogger<ReferencesNugetPackageOnlyOnce> _logger;

    public ReferencesNugetPackageOnlyOnce(ILogger<ReferencesNugetPackageOnlyOnce> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        HashSet<string> packageReferences = new(StringComparer.OrdinalIgnoreCase);

        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes == null)
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

            if (!packageReferences.Add(packageName))
            {
                this._logger.LogError($"{projectName}: Already references package {packageName}.");
            }
        }
    }
}