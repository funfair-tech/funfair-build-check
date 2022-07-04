using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the NSubstitute analyzer is installed for test projects.
/// </summary>
public sealed class UsingNSubstituteMustIncludeAnalyzer : HasAppropriateAnalysisPackages
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingNSubstituteMustIncludeAnalyzer(ILogger<UsingNSubstituteMustIncludeAnalyzer> logger)
        : base(detectPackageId: @"NSubstitute", mustIncludePackageId: @"NSubstitute.Analyzers.CSharp", logger: logger)
    {
    }
}