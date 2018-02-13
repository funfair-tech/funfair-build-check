using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class DotNetXUnitRunnerIsSameVersionAsPackage : IProjectCheck
    {
        private readonly ILogger<DotNetXUnitRunnerIsSameVersionAsPackage> _logger;

        public DotNetXUnitRunnerIsSameVersionAsPackage(ILogger<DotNetXUnitRunnerIsSameVersionAsPackage> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Check(string projectName, XmlDocument project)
        {
            XmlElement node = (XmlElement) project.SelectSingleNode("Project/ItemGroup/DotNetCliToolReference");
            if (node == null) return;

            string version = node.GetAttribute("Version");
            foreach (XmlElement projectReference in project.SelectNodes("Project/ItemGroup/PackageReference"))
            {
                string name = projectReference.GetAttribute("Include");
                if (StringComparer.InvariantCultureIgnoreCase.Equals(name, "xunit"))
                {
                    string refVersion = projectReference.GetAttribute("Version");
                    if (!StringComparer.InvariantCultureIgnoreCase.Equals(version, refVersion))
                        this._logger.LogError($"{projectName}: XUnit Reference ({name}) has version {refVersion} whereas DotNetCliToolReference has reference {version}.");

                    return;
                }
            }
        }
    }
}