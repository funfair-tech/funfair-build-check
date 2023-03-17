using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the C# Sonar Analyzer issues analyzer is installed.
/// </summary>
public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSonarAnalyzerPackage(ILogger<MustHaveSonarAnalyzerPackage> logger)
        : base(packageId: @"SonarAnalyzer.CSharp", mustHave: true, logger: logger)
    {
    }
}