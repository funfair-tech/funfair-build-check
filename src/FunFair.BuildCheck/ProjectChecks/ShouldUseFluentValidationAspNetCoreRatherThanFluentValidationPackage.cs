using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that the correct version of FluentValidation is installed.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class ShouldUseFluentValidationAspNetCoreRatherThanFluentValidationPackage : ShouldUseAbstractionsPackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public ShouldUseFluentValidationAspNetCoreRatherThanFluentValidationPackage(ILogger<ShouldUseFluentValidationAspNetCoreRatherThanFluentValidationPackage> logger)
            : base(matchPackageId: @"FluentValidation", usePackageId: @"FluentValidation.AspNetCore", logger: logger)
        {
        }
    }
}