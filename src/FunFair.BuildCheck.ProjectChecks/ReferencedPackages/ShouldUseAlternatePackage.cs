using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class ShouldUseAlternatePackage : IProjectCheck
{
    private readonly string _matchPackageId;
    private readonly string _usePackageId;

    protected ShouldUseAlternatePackage(string matchPackageId, string usePackageId, ILogger logger)
    {
        this._matchPackageId = matchPackageId;
        this._usePackageId = usePackageId;
        this.Logger = logger;
    }

    protected ILogger Logger { get; }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!this.CanCheck(project: project))
        {
            return ValueTask.CompletedTask;
        }

        string outputType = project.GetOutputType();

        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: outputType, y: "Exe"))
        {
            // Executables can use whatever they want.
            return ValueTask.CompletedTask;
        }

        string? awsProjectType = project.GetAwsProjectType();

        if (
            awsProjectType is not null
            && StringComparer.InvariantCultureIgnoreCase.Equals(x: awsProjectType, y: "Lambda")
        )
        {
            // Lambdas are effectively executables so can use whatever they want.
            return ValueTask.CompletedTask;
        }

        if (this.ShouldExclude(project: project, logger: this.Logger))
        {
            // Test projects can use whatever they want.
            return ValueTask.CompletedTask;
        }

        if (project.ReferencesPackage(this._matchPackageId, this.Logger))
        {
            this.Logger.UseAlternatePackageIdForMatchedPackageId(
                projectName: project.Name,
                usePackageId: this._usePackageId,
                matchPackageId: this._matchPackageId
            );
        }

        return ValueTask.CompletedTask;
    }

    protected virtual bool CanCheck(in ProjectContext project)
    {
        return true;
    }

    protected virtual bool ShouldExclude(in ProjectContext project, ILogger logger)
    {
        return false;
    }
}
