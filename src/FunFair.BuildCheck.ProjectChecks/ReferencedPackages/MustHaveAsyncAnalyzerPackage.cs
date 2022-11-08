using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the async issues analyzer is installed.
/// </summary>
public sealed class MustHaveAsyncAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveAsyncAnalyzerPackage(ILogger<MustHaveAsyncAnalyzerPackage> logger)
        : base(packageId: @"AsyncFixer", logger: logger)
    {
    }
}


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
        : base(packageId: @"codecracker.CSharp", logger: logger)
    {
    }
}

/// <summary>
///     Checks that the enum source generator and issues analyzer is installed.
/// </summary>
public sealed class MustHaveEnumSourceGeneratorAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveEnumSourceGeneratorAnalyzerPackage(ILogger<MustHaveEnumSourceGeneratorAnalyzerPackage> logger)
        : base(packageId: @"Credfeto.Enumeration.Source.Generation", logger: logger)
    {
    }
}

/// <summary>
///     Checks that the Meziantou issues analyzer is installed.
/// </summary>
public sealed class MustHaveMeziantouAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveMeziantouAnalyzerPackage(ILogger<MustHaveMeziantouAnalyzerPackage> logger)
        : base(packageId: @"Meziantou.Analyzer", logger: logger)
    {
    }
}

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
        : base(packageId: @"Philips.CodeAnalysis.DuplicateCodeAnalyzer", logger: logger)
    {
    }
}

/// <summary>
///     Checks that the Security Code Scan code issues analyzer is installed.
/// </summary>
public sealed class MustHaveSecurityCodeScanAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSecurityCodeScanAnalyzerPackage(ILogger<MustHaveSecurityCodeScanAnalyzerPackage> logger)
        : base(packageId: @"SecurityCodeScan.VS2019", logger: logger)
    {
    }
}

/// <summary>
///     Checks that the Smart issues analyzer is installed.
/// </summary>
public sealed class MustHaveSmartAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHaveSmartAnalyzerPackage(ILogger<MustHaveSmartAnalyzerPackage> logger)
        : base(packageId: @"SmartAnalyzers.CSharpExtensions.Annotations", logger: logger)
    {
    }
}

/// <summary>
///     Checks that the Philips maintainability issues analyzer is installed.
/// </summary>
public sealed class MustHavePhilipsMaintainabilityAnalyzerPackage : MustHaveAnalyzerPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public MustHavePhilipsMaintainabilityAnalyzerPackage(ILogger<MustHavePhilipsMaintainabilityAnalyzerPackage> logger)
        : base(packageId: @"Philips.CodeAnalysis.MaintainabilityAnalyzers", logger: logger)
    {
    }
}