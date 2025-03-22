using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class MustNotReferencePackages : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly IReadOnlyList<string> _packageIds;
    private readonly string _reason;

    protected MustNotReferencePackages(
        IReadOnlyList<string> packageIds,
        string reason,
        ILogger logger
    )
    {
        this._packageIds = packageIds;
        this._reason = reason;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        foreach (string packageId in this._packageIds)
        {
            bool packageExists = CheckReference(packageId: packageId, project: project);

            if (packageExists)
            {
                this._logger.ReferencesObsoletedPackageUsingNuGet(
                    projectName: project.Name,
                    packageId: packageId,
                    reason: this._reason
                );
            }
        }

        return ValueTask.CompletedTask;
    }

    private static bool CheckReference(string packageId, in ProjectContext project)
    {
        return project.CsProjXml.SelectSingleNode(
                "/Project/ItemGroup/PackageReference[@Include='" + packageId + "']"
            ) is XmlElement;
    }
}
