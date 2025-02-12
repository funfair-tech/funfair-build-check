using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsDependencyInjectionPackage : ShouldUseAlternatePackage
{
    private readonly IRepositorySettings _repositorySettings;

    public ShouldUseAbstractionsDependencyInjectionPackage(IRepositorySettings repositorySettings, ILogger<ShouldUseAbstractionsDependencyInjectionPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.DependencyInjection", usePackageId: "Microsoft.Extensions.DependencyInjection.Abstractions", logger: logger)
    {
        this._repositorySettings = repositorySettings;
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return !this.IsUnitTestBaseProject(projectName: projectName, project: project);
    }

    private bool IsUnitTestBaseProject(string projectName, XmlDocument project)
    {
        return this._repositorySettings.IsUnitTestBase
            && project.IsTestProject(projectName: projectName, logger: this.Logger)
            && !projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
    }
}
