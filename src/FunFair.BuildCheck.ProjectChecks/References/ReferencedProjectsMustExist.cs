using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.References.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class ReferencedProjectsMustExist : IProjectCheck
{
    private readonly ILogger<ReferencedProjectsMustExist> _logger;

    public ReferencedProjectsMustExist(ILogger<ReferencedProjectsMustExist> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(
        string projectName,
        string projectFolder,
        XmlDocument project,
        CancellationToken cancellationToken
    )
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/ProjectReference");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string projectReference = reference.GetAttribute(name: "Include");

            string referencedProject = Path.Combine(
                path1: projectFolder,
                PathHelpers.ConvertToNative(projectReference)
            );
            FileInfo i = new(referencedProject);

            if (!i.Exists)
            {
                this._logger.ReferencesProjectThatDoesNotExist(
                    projectName: projectName,
                    referencedProject: referencedProject
                );
            }
        }

        return ValueTask.CompletedTask;
    }
}
