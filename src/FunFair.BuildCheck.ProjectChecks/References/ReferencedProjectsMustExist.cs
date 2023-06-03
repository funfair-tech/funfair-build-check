using System.IO;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.References;

public sealed class ReferencedProjectsMustExist : IProjectCheck
{
    private readonly ILogger<ReferencedProjectsMustExist> _logger;

    public ReferencedProjectsMustExist(ILogger<ReferencedProjectsMustExist> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/ProjectReference");

        if (nodes is null)
        {
            return;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string projectReference = reference.GetAttribute(name: "Include");

            string referencedProject = Path.Combine(path1: projectFolder, PathHelpers.ConvertToNative(projectReference));
            FileInfo i = new(referencedProject);

            if (!i.Exists)
            {
                this._logger.LogError($"Project {projectName} references {referencedProject} that does not exist.");
            }
        }
    }
}