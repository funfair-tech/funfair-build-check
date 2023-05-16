using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class MustHaveRelatedPackage : IProjectCheck
{
    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected MustHaveRelatedPackage(string detectPackageId, string mustIncludePackageId, ILogger logger)
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        bool foundSourcePackage = false;
        bool foundRelatedPackage = false;

        if (nodes != null)
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
                    foundRelatedPackage = true;
                }
            }
        }

        if (foundSourcePackage && !foundRelatedPackage)
        {
            this._logger.LogError($"{projectName}: Found {this._detectPackageId} but did not find required {this._mustIncludePackageId}.");
        }
    }
}