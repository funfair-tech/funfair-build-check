using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the Test SDK is installed for test projects.
    /// </summary>

    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class UsingXUnitMustIncludeVisualStudioTestPlatform : HasAppropriateNonAnalysisPackages
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public UsingXUnitMustIncludeVisualStudioTestPlatform(ILogger<UsingXUnitMustIncludeVisualStudioTestPlatform> logger)
            : base(detectPackageId: @"xunit", mustIncludePackageId: @"Microsoft.NET.Test.Sdk", logger: logger)
        {
        }
    }
}
