using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

/// <summary>
///     Checks that libraries do not depend on non-packable items.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class PackableLibrariesShouldNotDependOnNonPackable : IProjectCheck
{
    private readonly ILogger<PackableLibrariesShouldNotDependOnNonPackable> _logger;
    private readonly IProjectLoader _projectLoader;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="projectLoader">Project loader.</param>
    /// <param name="logger">Logging.</param>
    public PackableLibrariesShouldNotDependOnNonPackable(IProjectLoader projectLoader, ILogger<PackableLibrariesShouldNotDependOnNonPackable> logger)
    {
        this._projectLoader = projectLoader ?? throw new ArgumentNullException(nameof(projectLoader));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: "Library", project.GetOutputType()))
        {
            return;
        }

        if (!project.IsPackable())
        {
            return;
        }

        XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/ProjectReference");

        if (nodes == null)
        {
            return;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string projectReference = reference.GetAttribute(name: "Include");

            string referencedProject = Path.Combine(path1: projectFolder, path2: projectReference);
            FileInfo i = new(referencedProject);

            if (!i.Exists)
            {
                continue;
            }

            referencedProject = i.FullName;

            XmlDocument otherProject = this._projectLoader.Load(referencedProject);

            if (!otherProject.IsPackable())
            {
                this._logger.LogError($"Packable Library project {projectName} references non-packable project {referencedProject}");
            }
        }
    }
}