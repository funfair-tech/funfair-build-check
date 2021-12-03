using System;
using System.Collections.Generic;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the specified selection of packages are not referenced.
/// </summary>
public abstract class MustNotReferencePackages : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly IReadOnlyList<string> _packageIds;
    private readonly string _reason;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="packageIds">The package ids that shouldn't be referenced.</param>
    /// <param name="reason">The reason they shouldn't be referenced.</param>
    /// <param name="logger">Logging.</param>
    protected MustNotReferencePackages(IReadOnlyList<string> packageIds, string reason, ILogger logger)
    {
        this._packageIds = packageIds ?? throw new ArgumentNullException(nameof(packageIds));
        this._reason = reason ?? throw new ArgumentNullException(nameof(reason));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        foreach (string packageId in this._packageIds)
        {
            bool packageExists = CheckReference(packageId: packageId, project: project);

            if (packageExists)
            {
                this._logger.LogError($"{projectName}: References obsoleted package {packageId} using NuGet. {this._reason}");
            }
        }
    }

    private static bool CheckReference(string packageId, XmlDocument project)
    {
        return project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") is XmlElement;
    }
}