using System;
using System.Xml;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace BuildCheck.ProjectChecks
{
    public class NoPreReleaseNuGetPackages : IProjectCheck
    {
        private readonly ICheckConfiguration _configuration;
        private readonly ILogger<NoPreReleaseNuGetPackages> _logger;

        public NoPreReleaseNuGetPackages(ICheckConfiguration configuration, ILogger<NoPreReleaseNuGetPackages> logger)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList nodes = project.SelectNodes("/Project/ItemGroup/PackageReference");
            foreach (XmlElement reference in nodes)
            {
                string packageName = reference.GetAttribute(@"Include");
                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");
                    continue;
                }

                string version = reference.GetAttribute(@"Version");

                this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

                if (!NuGetVersion.TryParse(version, out NuGetVersion nuGetVersion))
                {
                    this._logger.LogError($"{projectName}: Package {packageName} could not parse version {version}.");
                    continue;
                }

                if (nuGetVersion.IsPrerelease && !this._configuration.PreReleaseBuild)
                    this._logger.LogError($"{projectName}: Package {packageName} uses pre-release version {version}.");
            }
        }
    }
}