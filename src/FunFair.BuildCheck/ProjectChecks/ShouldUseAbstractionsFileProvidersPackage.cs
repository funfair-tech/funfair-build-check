using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class ShouldUseAbstractionsFileProvidersPackage : ShouldUseAbstractionsPackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseAbstractionsFileProvidersPackage(ILogger<ShouldUseAbstractionsLoggingPackage> logger)
            : base(matchPackageId: @"Microsoft.Extensions.FileProviders", usePackageId: @"Microsoft.Extensions.FileProviders.Abstractions", logger: logger)
        {
        }
    }
}