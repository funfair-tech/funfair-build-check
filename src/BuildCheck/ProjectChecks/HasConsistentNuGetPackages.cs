using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace BuildCheck.ProjectChecks
{
    public sealed class HasConsistentNuGetPackages : IProjectCheck
    {
        private readonly ILogger<NoPreReleaseNuGetPackages> _logger;

        private readonly Dictionary<string, NuGetVersion> _packages;

        public HasConsistentNuGetPackages(ILogger<NoPreReleaseNuGetPackages> logger)
        {
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