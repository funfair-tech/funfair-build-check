using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseAbstractionsConfigurationPackage : ShouldUseAlternatePackage
{
    private readonly IRepositorySettings _repositorySettings;

    public ShouldUseAbstractionsConfigurationPackage(IRepositorySettings repositorySettings, ILogger<ShouldUseAbstractionsConfigurationPackage> logger)
        : base(matchPackageId: "Microsoft.Extensions.Configuration", usePackageId: "Microsoft.Extensions.Configuration.Abstractions", logger: logger)
    {
        this._repositorySettings = repositorySettings;
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return !this._repositorySettings.IsUnitTestBase;
    }
}