using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that NSubstitute is used rather than Moq
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class ShouldUseNSubstituteRatherThanMoqPackage : ShouldUseAlternatePackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public ShouldUseNSubstituteRatherThanMoqPackage(ILogger<ShouldUseNSubstituteRatherThanMoqPackage> logger)
        : base(matchPackageId: @"Moq", usePackageId: @"NSubstitute", logger: logger)
    {
    }
}