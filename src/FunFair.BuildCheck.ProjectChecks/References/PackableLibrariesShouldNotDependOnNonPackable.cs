using System;
using System.IO;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class PackableLibrariesShouldNotDependOnNonPackable : IProjectCheck
{
    private readonly ILogger<PackableLibrariesShouldNotDependOnNonPackable> _logger;
    private readonly IProjectLoader _projectLoader;

    public PackableLibrariesShouldNotDependOnNonPackable(IProjectLoader projectLoader, ILogger<PackableLibrariesShouldNotDependOnNonPackable> logger)
    {
        this._projectLoader = projectLoader;
        this._logger = logger;
    }

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