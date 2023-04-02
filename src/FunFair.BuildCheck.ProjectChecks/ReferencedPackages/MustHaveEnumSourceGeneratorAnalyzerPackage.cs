using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveEnumSourceGeneratorAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveEnumSourceGeneratorAnalyzerPackage(IRepositorySettings repositorySettings, ILogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger)
        : base(packageId: @"Credfeto.Enumeration.Source.Generation", mustHave: repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage, logger: logger)
    {
    }
}