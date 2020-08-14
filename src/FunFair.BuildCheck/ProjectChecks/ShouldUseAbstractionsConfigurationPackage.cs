using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class ShouldUseAbstractionsConfigurationPackage : ShouldUseAbstractionsPackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsConfigurationPackage(ILogger<ShouldUseAbstractionsLoggingPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.Configuration", usePackageId: @"Microsoft.Extensions.Configuration.Abstractions", logger: logger)
        {
        }
    }
}