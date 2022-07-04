using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that nuget *.All packages are not referenced in a packable project.
/// </summary>
public sealed class ShouldNotReferenceAllMetaPackagesInPackableProjects : IProjectCheck
{
    private readonly ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ShouldNotReferenceAllMetaPackagesInPackableProjects(ILogger<ShouldNotReferenceAllMetaPackagesInPackableProjects> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

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
                continue;
            }

            if (packageName.EndsWith(value: ".All", comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                this._logger.LogError(
                    $"{projectName}: References meta-package {packageName} rather than the individual packages -> It needs to use individual packages to control the nuget dependencies for users of the published package.");
            }
        }
    }
}