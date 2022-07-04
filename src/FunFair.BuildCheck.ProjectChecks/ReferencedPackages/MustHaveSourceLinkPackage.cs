using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the SourceLink package is installed.
/// </summary>
public sealed class MustHaveSourceLinkPackage : IProjectCheck
{
    private const string HISTORICAL_PACKAGE_ID = @"SourceLink.Create.CommandLine";
    private const string PACKAGE_ID = @"Microsoft.SourceLink.GitHub";
    private const string PACKAGE_PRIVATE_ASSETS = @"All";

    private readonly ILogger<MustHaveSourceLinkPackage> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSourceLinkPackage(ILogger<MustHaveSourceLinkPackage> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (project.SelectSingleNode(xpath: "/Project/ItemGroup/PackageReference[@Include='xunit']") is XmlElement)
        {
            // has an xunit reference so is a unit test project, don't force sourcelink
            return;
        }

        bool packageExists = CheckReference(packageId: PACKAGE_ID, project: project);
        bool historicalPackageExists = CheckReference(packageId: HISTORICAL_PACKAGE_ID, project: project);

        if (!packageExists && !historicalPackageExists)
        {
            this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} or {HISTORICAL_PACKAGE_ID} using NuGet");
        }

        if (packageExists && historicalPackageExists)
        {
            this._logger.LogError($"{projectName}: References both {PACKAGE_ID} and {HISTORICAL_PACKAGE_ID}");
        }

        if (packageExists)
        {
            if (!CheckPrivateAssets(packageId: PACKAGE_ID, project: project))
            {
                this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }
        }

        if (historicalPackageExists)
        {
            if (!CheckPrivateAssets(packageId: HISTORICAL_PACKAGE_ID, project: project))
            {
                this._logger.LogError($"{projectName}: Does not reference {HISTORICAL_PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }
        }
    }

    private static bool CheckReference(string packageId, XmlDocument project)
    {
        XmlElement? reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;

        return reference != null;
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