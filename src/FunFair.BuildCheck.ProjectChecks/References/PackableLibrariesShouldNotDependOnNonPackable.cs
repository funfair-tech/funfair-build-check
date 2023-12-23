using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class PackableLibrariesShouldNotDependOnNonPackable : IProjectCheck
{
    private readonly ILogger<PackableLibrariesShouldNotDependOnNonPackable> _logger;
    private readonly IProjectXmlLoader _projectXmlLoader;

    public PackableLibrariesShouldNotDependOnNonPackable(IProjectXmlLoader projectXmlLoader, ILogger<PackableLibrariesShouldNotDependOnNonPackable> logger)
    {
        this._projectXmlLoader = projectXmlLoader;
        this._logger = logger;
    }

    public async ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
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

        if (nodes is null)
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

            XmlDocument otherProject = await this._projectXmlLoader.LoadAsync(referencedProject, cancellationToken);

            if (!otherProject.IsPackable())
            {
                this._logger.LogError($"Packable Library project {projectName} references non-packable project {referencedProject}");
            }
        }
    }
}