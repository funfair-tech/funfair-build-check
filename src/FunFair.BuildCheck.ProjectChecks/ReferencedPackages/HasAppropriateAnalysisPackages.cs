using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class HasAppropriateAnalysisPackages : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = @"All";

    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected HasAppropriateAnalysisPackages(string detectPackageId, string mustIncludePackageId, ILogger logger)
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        bool foundSourcePackage = false;
        bool foundAnalyzerPackage = false;

        if (nodes is not null)
        {
            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._detectPackageId, y: packageName))
                {
                    foundSourcePackage = true;
                }

                if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._mustIncludePackageId, y: packageName))
                {
                    foundAnalyzerPackage = true;
                    string assets = reference.GetAttribute(name: "PrivateAssets");

                    if (string.IsNullOrWhiteSpace(assets) || assets != PACKAGE_PRIVATE_ASSETS)
                    {
                        this._logger.LogError($"{projectName}: Does not reference {this._mustIncludePackageId} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
                    }
                }
            }
        }

        if (foundSourcePackage && !foundAnalyzerPackage)
        {
            this._logger.LogError($"{projectName}: Found {this._detectPackageId} but did not find analyzer {this._mustIncludePackageId}.");
        }
    }
}