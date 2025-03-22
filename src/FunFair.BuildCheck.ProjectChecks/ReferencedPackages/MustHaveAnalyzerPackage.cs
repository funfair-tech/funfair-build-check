using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class MustHaveAnalyzerPackage : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = "All";
    private const string PACKAGE_EXCLUDE_ASSETS = "runtime";
    private readonly ILogger _logger;
    private readonly bool _mustHave;

    private readonly string _packageId;

    protected MustHaveAnalyzerPackage(string packageId, bool mustHave, ILogger logger)
    {
        this._packageId = packageId;
        this._mustHave = mustHave;
        this._logger = logger;
    }

    protected virtual bool CanCheck(in ProjectContext project)
    {
        return true;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!this._mustHave)
        {
            return ValueTask.CompletedTask;
        }

        if (!this.CanCheck(project))
        {
            return ValueTask.CompletedTask;
        }

        bool packageExists = project.ReferencesPackage(this._packageId, logger: this._logger);

        if (packageExists)
        {
            if (!CheckPrivateAssets(packageId: this._packageId, project: project))
            {
                this._logger.DoesNotUsePrivateAssetsAttribute(
                    projectName: project.Name,
                    packageId: this._packageId,
                    privateAssets: PACKAGE_PRIVATE_ASSETS
                );
            }

            if (!(project.IsFunFairTestProject() && IsPackageExcluded(packageId: this._packageId)))
            {
                if (!CheckExcludeAssets(packageId: this._packageId, project: project))
                {
                    this._logger.DoesNotUsePrivateAssetsAttribute(
                        projectName: project.Name,
                        packageId: this._packageId,
                        privateAssets: PACKAGE_PRIVATE_ASSETS
                    );
                }
            }
        }
        else
        {
            this._logger.DoesNotUseNuGet(projectName: project.Name, packageId: this._packageId);
        }

        return ValueTask.CompletedTask;
    }

    private static bool IsPackageExcluded(string packageId)
    {
        return StringComparer.InvariantCultureIgnoreCase.Equals(
                x: packageId,
                y: "Microsoft.NET.Test.Sdk"
            )
            || StringComparer.InvariantCultureIgnoreCase.Equals(
                x: packageId,
                y: "xunit.runner.visualstudio"
            );
    }

    private static bool CheckPrivateAssets(string packageId, in ProjectContext project)
    {
        if (
            project.CsProjXml.SelectSingleNode(
                "/Project/ItemGroup/PackageReference[@Include='" + packageId + "']"
            )
            is not XmlElement reference
        )
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

        return !string.IsNullOrEmpty(assets)
            && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS);
    }

    private static bool CheckExcludeAssets(string packageId, in ProjectContext project)
    {
        if (
            project.CsProjXml.SelectSingleNode(
                "/Project/ItemGroup/PackageReference[@Include='" + packageId + "']"
            )
            is not XmlElement reference
        )
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

        return !string.IsNullOrEmpty(assets)
            && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_EXCLUDE_ASSETS);
    }
}
