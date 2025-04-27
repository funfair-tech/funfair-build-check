using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsDependencyInjectionPackage : ShouldUseAlternatePackage
{
    private readonly IRepositorySettings _repositorySettings;

    public ShouldUseAbstractionsDependencyInjectionPackage(
        IRepositorySettings repositorySettings,
        ILogger<ShouldUseAbstractionsDependencyInjectionPackage> logger
    )
        : base(
            matchPackageId: "Microsoft.Extensions.DependencyInjection",
            usePackageId: "Microsoft.Extensions.DependencyInjection.Abstractions",
            logger: logger
        )
    {
        this._repositorySettings = repositorySettings;
    }

    protected override bool CanCheck(in ProjectContext project)
    {
        return !this.IsUnitTestBaseProject(project: project);
    }

    private bool IsUnitTestBaseProject(in ProjectContext project)
    {
        return this._repositorySettings.IsUnitTestBase
            && project.IsTestProject(logger: this.Logger)
            && !project.Name.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
    }
}
