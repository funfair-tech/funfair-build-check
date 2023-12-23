using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class HasConsistentNuGetPackages : IProjectCheck
{
    private readonly ICheckConfiguration _checkConfiguration;
    private readonly ILogger<HasConsistentNuGetPackages> _logger;

    private readonly Dictionary<string, NuGetVersion> _packages;

    public HasConsistentNuGetPackages(ICheckConfiguration checkConfiguration, ILogger<HasConsistentNuGetPackages> logger)
    {
        this._checkConfiguration = checkConfiguration;
        this._logger = logger;

        this._packages = new(StringComparer.OrdinalIgnoreCase);
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                continue;
            }

            string version = reference.GetAttribute(name: "Version");

            this._logger.LogDebug($"{projectName}: Found: {packageName} ({version})");

            if (!NuGetVersion.TryParse(value: version, out NuGetVersion? nuGetVersion))
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
                if (this._checkConfiguration.AllowPackageVersionMismatch)
                {
                    this._logger.LogInformation($"{projectName}: Uses {packageName} version {nuGetVersion} when it should be using previously seen {currentVersion}.");
                }
                else
                {
                    this._logger.LogError($"{projectName}: Uses {packageName} version {nuGetVersion} when it should be using previously seen {currentVersion}.");
                }

                continue;
            }

            this._logger.LogInformation($"{projectName}: Uses {packageName} version {nuGetVersion}.");
        }

        return ValueTask.CompletedTask;
    }
}