using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the xunit analyzer is installed for test projects.
    /// </summary>


    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class UsingXUnitMustIncludeAnalyzer : HasAppropriateAnalysisPackages
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public UsingXUnitMustIncludeAnalyzer(ILogger<UsingXUnitMustIncludeAnalyzer> logger)
            : base(detectPackageId: @"xunit", mustIncludePackageId: @"xunit.analyzers", logger: logger)
        {
        }
    }
}

