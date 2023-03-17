using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the codecracker.CSharp issues analyzer is installed.
/// </summary>
public sealed class MustHaveCodecrackerCSharpAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveCodecrackerCSharpAnalyzerPackage(ILogger<MustHaveCodecrackerCSharpAnalyzerPackage> logger)
        : base(packageId: @"codecracker.CSharp", mustHave: true, logger: logger)
    {
    }
}