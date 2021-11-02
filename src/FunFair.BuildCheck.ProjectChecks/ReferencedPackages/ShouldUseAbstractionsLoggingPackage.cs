using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the logging abstractions package is used rather than the full version.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldUseAbstractionsLoggingPackage : ShouldUseAlternatePackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsLoggingPackage(ILogger<ShouldUseAbstractionsLoggingPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.Logging", usePackageId: @"Microsoft.Extensions.Logging.Abstractions", logger: logger)
        {
        }
    }
}