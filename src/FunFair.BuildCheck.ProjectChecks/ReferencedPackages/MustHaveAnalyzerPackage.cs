using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that global analyzer packages are installed.
/// </summary>
public abstract class MustHaveAnalyzerPackage : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = @"All";
    private const string PACKAGE_EXCLUDE_ASSETS = @"runtime";
    private readonly ILogger _logger;
    private readonly bool _mustHave;

    private readonly string _packageId;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="packageId">The package that must be installed.</param>
    /// <param name="logger">Logging.</param>
    protected MustHaveAnalyzerPackage(string packageId, bool mustHave, ILogger logger)
    {
        this._packageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
        this._mustHave = mustHave;
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!this._mustHave)
        {
            return;
        }

        bool packageExists = CheckReference(packageId: this._packageId, project: project);

        if (packageExists)
        {
            if (!CheckPrivateAssets(packageId: this._packageId, project: project))
            {
                this._logger.LogError($"{projectName}: Does not reference {this._packageId} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }

            if (!(project.IsFunFairTestProject() && IsPackageExcluded(packageId: this._packageId)))
            {
                if (!CheckExcludeAssets(packageId: this._packageId, project: project))
                {
                    this._logger.LogError($"{projectName}: Does not reference {this._packageId} with a ExcludeAssets=\"{PACKAGE_EXCLUDE_ASSETS}\" attribute");
                }
            }
        }
        else
        {
            this._logger.LogError($"{projectName}: Does not reference {this._packageId} using NuGet");
        }
    }

    private static bool IsPackageExcluded(string packageId)
    {
        return StringComparer.InvariantCultureIgnoreCase.Equals(x: packageId, y: "Microsoft.NET.Test.Sdk") ||
               StringComparer.InvariantCultureIgnoreCase.Equals(x: packageId, y: "xunit.runner.visualstudio");
    }

    private static bool CheckReference(string packageId, XmlDocument project)
    {
        return project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") is XmlElement;
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

        return !string.IsNullOrEmpty(assets) && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS);
    }

    private static bool CheckExcludeAssets(string packageId, XmlDocument project)
    {
        if (project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") is not XmlElement reference)
        {
            return false;
        }

        // check for an attribute
        string assets = reference.GetAttribute(name: "ExcludeAssets");

        if (string.IsNullOrEmpty(assets))
        {
            // no PrivateAssets attribute, check for an element
            if (reference.SelectSingleNode(xpath: "ExcludeAssets") is not XmlElement privateAssets)
            {
                return false;
            }

            assets = privateAssets.InnerText;
        }

        return !string.IsNullOrEmpty(assets) && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_EXCLUDE_ASSETS);
    }
}