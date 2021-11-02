using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the hosting abstractions package is used rather than the full version.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldUseAbstractionsExtensionHostingPackage : ShouldUseAlternatePackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsExtensionHostingPackage(ILogger<ShouldUseAbstractionsExtensionHostingPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.Hosting", usePackageId: @"Microsoft.Extensions.Hosting.Abstractions", logger: logger)
        {
        }
    }
}