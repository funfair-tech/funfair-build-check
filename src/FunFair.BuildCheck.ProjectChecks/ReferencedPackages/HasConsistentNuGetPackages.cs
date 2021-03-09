using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that nuget packages are set to a consistent version across an entire solution.
    /// </summary>

    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class HasConsistentNuGetPackages : IProjectCheck
    {
        private readonly ILogger<HasConsistentNuGetPackages> _logger;

        private readonly Dictionary<string, NuGetVersion> _packages;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public HasConsistentNuGetPackages(ILogger<HasConsistentNuGetPackages> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this._packages = new Dictionary<string, NuGetVersion>();
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            if (nodes == null)
            {
                return;
            }

            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                string version = reference.GetAttribute(name: @"Version");

                this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

                if (!NuGetVersion.TryParse(value: version, out NuGetVersion nuGetVersion))
                {
                    this._logger.LogError($"{projectName}: Package {packageName} could not parse version {version}.");

                    continue;
                }

                string packageAsKey = packageName.ToLowerInvariant();

                if (!this._packages.TryGetValue(key: packageAsKey, out NuGetVersion? currentVersion))
                {
                    this._packages.Add(key: packageAsKey, value: nuGetVersion);
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
