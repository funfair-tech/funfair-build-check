using System;
using System.Collections.Generic;
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

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            foreach (XmlElement reference in nodes)
            {
                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                string version = reference.GetAttribute(name: @"Version");

                this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

                if (!NuGetVersion.TryParse(version, out NuGetVersion nuGetVersion))
                {
                    this._logger.LogError($"{projectName}: Package {packageName} could not parse version {version}.");

                    continue;
                }

                if (nuGetVersion.IsPrerelease && !this._configuration.PreReleaseBuild)
                {
                    this._logger.LogError($"{projectName}: Package {packageName} uses pre-release version {version}.");
                }
            }
        }
    }

    public class HasConsistentNuGetPackages : IProjectCheck
    {
        private readonly ICheckConfiguration _configuration;
        private readonly ILogger<NoPreReleaseNuGetPackages> _logger;

        private readonly Dictionary<string, NuGetVersion> _packages;

        public HasConsistentNuGetPackages(ICheckConfiguration configuration, ILogger<NoPreReleaseNuGetPackages> logger)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this._packages = new Dictionary<string, NuGetVersion>();
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            foreach (XmlElement reference in nodes)
            {
                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                string version = reference.GetAttribute(name: @"Version");

                this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

                if (!NuGetVersion.TryParse(version, out NuGetVersion nuGetVersion))
                {
                    this._logger.LogError($"{projectName}: Package {packageName} could not parse version {version}.");

                    continue;
                }

                string packageAsKey = packageName.ToLowerInvariant();

                if (!this._packages.TryGetValue(packageAsKey, out NuGetVersion currentVersion))
                {
                    this._packages.Add(packageAsKey, nuGetVersion);
                }
                else if (currentVersion != nuGetVersion)
                {
                    this._logger.LogError($"{projectName}: Uses {packageName} version {nuGetVersion} when it should be using previously seen {currentVersion}.");

                    continue;
                }

                this._logger.LogInformation($"{projectName}: Uses {packageName} version {nuGetVersion}.");
            }
        }
    }
}