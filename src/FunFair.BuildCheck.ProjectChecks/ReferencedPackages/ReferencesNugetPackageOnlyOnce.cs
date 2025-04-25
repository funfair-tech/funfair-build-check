using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Models;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ReferencesNugetPackageOnlyOnce : IProjectCheck
{
    private const string PACKAGE_PRIVATE_ASSETS = "All";
    private readonly ILogger<ReferencesNugetPackageOnlyOnce> _logger;

    public ReferencesNugetPackageOnlyOnce(ILogger<ReferencesNugetPackageOnlyOnce> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        HashSet<string> packageReferences = new(StringComparer.OrdinalIgnoreCase);

        foreach (PackageReference package in project.ReferencedPackageElements(this._logger))
        {
            // check for private asset (if it's private the build won't fail for a pre-release package)
            string? privateAssets = package.GetAttributeOrElement("PrivateAssets");

            if (!string.IsNullOrEmpty(privateAssets) && StringComparer.OrdinalIgnoreCase.Equals(x: privateAssets, y: PACKAGE_PRIVATE_ASSETS))
            {
                continue;
            }

            if (!packageReferences.Add(package.Id))
            {
                this._logger.AlreadyReferencesPackage(projectName: project.Name, packageId: package.Id);
            }
        }

        return ValueTask.CompletedTask;
    }
}