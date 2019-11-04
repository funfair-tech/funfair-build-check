using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public sealed class DotNetXUnitRunnerIsSameVersionAsPackage : IProjectCheck
    {
        private readonly ILogger<DotNetXUnitRunnerIsSameVersionAsPackage> _logger;

        public DotNetXUnitRunnerIsSameVersionAsPackage(ILogger<DotNetXUnitRunnerIsSameVersionAsPackage> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlElement node = (XmlElement) project.SelectSingleNode(xpath: "Project/ItemGroup/DotNetCliToolReference");

            if (node == null)
            {
                return;
            }

            string version = node.GetAttribute(name: "Version");

            foreach (XmlElement? projectReference in project.SelectNodes(xpath: "Project/ItemGroup/PackageReference"))
            {
                if (projectReference == null)
                {
                    continue;
                }

                string name = projectReference.GetAttribute(name: "Include");

                if (StringComparer.InvariantCultureIgnoreCase.Equals(name, y: "xunit"))
                {
                    string refVersion = projectReference.GetAttribute(name: "Version");

                    if (!StringComparer.InvariantCultureIgnoreCase.Equals(version, refVersion))
                    {
                        this._logger.LogError($"{projectName}: XUnit Reference ({name}) has version {refVersion} whereas DotNetCliToolReference has reference {version}.");
                    }

                    return;
                }
            }
        }
    }
}