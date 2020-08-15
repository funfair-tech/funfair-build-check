namespace FunFair.BuildCheck.ProjectChecks
{
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