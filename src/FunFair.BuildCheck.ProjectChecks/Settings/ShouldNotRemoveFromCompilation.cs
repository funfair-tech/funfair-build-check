using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class ShouldNotRemoveFromCompilation : IProjectCheck
{
    private readonly ILogger<ShouldNotRemoveFromCompilation> _logger;
#if FALSE
    <ItemGroup>
        <Compile Remove
 = "Models\BlockRangeDto.cs" />
    </ItemGroup>
#endif
    public ShouldNotRemoveFromCompilation(ILogger<ShouldNotRemoveFromCompilation> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/Compile[@Remove]");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string projectReference = reference.GetAttribute(name: "Remove");
            this._logger.RemovesProjectReferenceFromCompilation(projectName: projectName, projectReference: projectReference);
        }

        return ValueTask.CompletedTask;
    }
}