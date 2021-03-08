using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the dependency injection abstractions package is used rather than the full version.
    /// </summary>

    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldUseAbstractionsDependencyInjectionPackage : ShouldUseAbstractionsPackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsDependencyInjectionPackage(ILogger<ShouldUseAbstractionsDependencyInjectionPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.DependencyInjection", usePackageId: @"Microsoft.Extensions.DependencyInjection.Abstractions", logger: logger)
        {
        }
    }
}
