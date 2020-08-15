namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class ShouldUseAbstractionsLoggingPackage : ShouldUseAbstractionsPackage
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