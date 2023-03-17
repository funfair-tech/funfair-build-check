using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the Duplicate code issues analyzer is installed.
/// </summary>
public sealed class MustHaveDuplicateCodeAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveDuplicateCodeAnalyzerPackage(ILogger<MustHaveDuplicateCodeAnalyzerPackage> logger)
        : base(packageId: @"Philips.CodeAnalysis.DuplicateCodeAnalyzer", mustHave: true, logger: logger)
    {
    }
}