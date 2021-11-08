using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that files are not removed from compilation.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldNotRemoveFromCompilation : IProjectCheck
    {
        private readonly ILogger<ShouldNotRemoveFromCompilation> _logger;
#if FALSE
    <ItemGroup>
        <Compile Remove = "Models\BlockRangeDto.cs" />
    </ItemGroup>
#endif
        public ShouldNotRemoveFromCompilation(ILogger<ShouldNotRemoveFromCompilation> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            XmlNodeList? nodes = project.SelectNodes("/Project/ItemGroup/Compile[@Remove]");

            if (nodes == null)
            {
                return;
            }

            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                string projectReference = reference.GetAttribute(name: "Remove");
                this._logger.LogError($"Removes {projectReference} from compilation");
            }
        }
    }
}