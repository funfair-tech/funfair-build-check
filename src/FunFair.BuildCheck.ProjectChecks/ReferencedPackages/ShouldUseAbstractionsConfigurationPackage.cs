using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the configuration abstractions package is used rather than the full version.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldUseAbstractionsConfigurationPackage : ShouldUseAlternatePackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsConfigurationPackage(ILogger<ShouldUseAbstractionsConfigurationPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.Configuration", usePackageId: @"Microsoft.Extensions.Configuration.Abstractions", logger: logger)
        {
        }
    }
}