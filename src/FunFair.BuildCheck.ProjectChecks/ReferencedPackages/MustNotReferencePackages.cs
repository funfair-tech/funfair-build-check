using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class MustNotReferencePackages : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly IReadOnlyList<string> _packageIds;
    private readonly string _reason;

    protected MustNotReferencePackages(IReadOnlyList<string> packageIds, string reason, ILogger logger)
    {
        this._packageIds = packageIds;
        this._reason = reason;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        foreach (string packageId in this._packageIds)
        {
            bool packageExists = project.ReferencesPackage(packageId, this._logger);

            if (packageExists)
            {
                this._logger.ReferencesObsoletedPackageUsingNuGet(projectName: project.Name, packageId: packageId, reason: this._reason);
            }
        }

        return ValueTask.CompletedTask;
    }
}