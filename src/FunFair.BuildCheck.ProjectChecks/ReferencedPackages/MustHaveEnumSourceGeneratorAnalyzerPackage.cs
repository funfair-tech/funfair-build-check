using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the enum source generator and issues analyzer is installed.
/// </summary>
public sealed class MustHaveEnumSourceGeneratorAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveEnumSourceGeneratorAnalyzerPackage(IRepositorySettings repositorySettings, ILogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger)
        : base(packageId: @"Credfeto.Enumeration.Source.Generation", mustHave: repositorySettings.MustHaveEnumSourceGeneratorAnalyzerPackage, logger: logger)
    {
    }
}