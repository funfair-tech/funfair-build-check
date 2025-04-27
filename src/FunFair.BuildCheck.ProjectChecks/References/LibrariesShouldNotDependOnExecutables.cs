using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class LibrariesShouldNotDependOnExecutables : IProjectCheck
{
    private readonly ILogger<LibrariesShouldNotDependOnExecutables> _logger;
    private readonly IProjectXmlLoader _projectXmlLoader;

    public LibrariesShouldNotDependOnExecutables(
        IProjectXmlLoader projectXmlLoader,
        ILogger<LibrariesShouldNotDependOnExecutables> logger
    )
    {
        this._projectXmlLoader = projectXmlLoader;
        this._logger = logger;
    }

    public async ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: "Library", project.GetOutputType()))
        {
            return;
        }

        XmlNodeList? nodes = project.CsProjXml.SelectNodes("/Project/ItemGroup/ProjectReference");

        if (nodes is null)
        {
            return;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string projectReference = reference.GetAttribute(name: "Include");

            string referencedProject = Path.Combine(path1: project.Folder, path2: projectReference);
            FileInfo i = new(referencedProject);

            if (!i.Exists)
            {
                continue;
            }

            referencedProject = i.FullName;

            XmlDocument otherProject = await this._projectXmlLoader.LoadAsync(
                path: referencedProject,
                cancellationToken: cancellationToken
            );

            // ! TODO: Change _projectXmlLoader to return ProjectContext
            ProjectContext op = new(i.Name, i.DirectoryName!, otherProject);

            string otherOutputType = op.GetOutputType();

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: "Exe", y: otherOutputType))
            {
                this._logger.LibraryReferencesExecutable(
                    projectName: project.Name,
                    referencedProject: referencedProject
                );
            }
        }
    }
}
