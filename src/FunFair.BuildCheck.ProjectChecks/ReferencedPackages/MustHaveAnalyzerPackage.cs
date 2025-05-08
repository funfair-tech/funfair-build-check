using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Models;
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

        bool packageExists = project.ReferencesPackage(packageName: this._packageId, logger: this._logger);

        if (packageExists)
        {
            if (!this.CheckPrivateAssets(packageId: this._packageId, project: project))
            {
                this._logger.DoesNotUsePrivateAssetsAttribute(projectName: project.Name, packageId: this._packageId, privateAssets: PACKAGE_PRIVATE_ASSETS);
            }

            if (!(project.IsFunFairTestProject() && IsPackageExcluded(packageId: this._packageId)))
            {
                if (!this.CheckExcludeAssets(packageId: this._packageId, project: project))
                {
                    this._logger.DoesNotUsePrivateAssetsAttribute(projectName: project.Name, packageId: this._packageId, privateAssets: PACKAGE_PRIVATE_ASSETS);
                }
            }
        }
        else
        {
            this._logger.DoesNotUseNuGet(projectName: project.Name, packageId: this._packageId);
        }

        return ValueTask.CompletedTask;
    }

    protected virtual bool CanCheck(in ProjectContext project)
    {
        return true;
    }

    private static bool IsPackageExcluded(string packageId)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(x: packageId, y: "Microsoft.NET.Test.Sdk") || StringComparer.OrdinalIgnoreCase.Equals(x: packageId, y: "xunit.runner.visualstudio");
    }

    private bool CheckPrivateAssets(string packageId, in ProjectContext project)
    {
        PackageReference? package = project.GetNamedReferencedPackage(packageId: packageId, logger: this._logger);

        if (package is null)
        {
            return false;
        }

        string? assets = package.Value.GetAttributeOrElement(attributeOrElementName: "PrivateAssets");

        return !string.IsNullOrEmpty(assets) && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_PRIVATE_ASSETS);
    }

    private bool CheckExcludeAssets(string packageId, in ProjectContext project)
    {
        PackageReference? package = project.GetNamedReferencedPackage(packageId: packageId, logger: this._logger);

        if (package is null)
        {
            return false;
        }

        string? assets = package.Value.GetAttributeOrElement(attributeOrElementName: "ExcludeAssets");

        return !string.IsNullOrEmpty(assets) && StringComparer.OrdinalIgnoreCase.Equals(x: assets, y: PACKAGE_EXCLUDE_ASSETS);
    }
}