using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldNotReferenceAllMetaPackagesInPackableProjects : IProjectCheck
{
    private readonly ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> _logger;

    public ShouldNotReferenceAllMetaPackagesInPackableProjects(ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        if (!project.IsPackable())
        {
            return ValueTask.CompletedTask;
        }

        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                continue;
            }

            if (packageName.EndsWith(value: ".All", comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                this._logger.DoNotReferenceMetaPackageInPackableProjects(projectName: projectName, packageId: packageName);
            }
        }

        return ValueTask.CompletedTask;
    }
}