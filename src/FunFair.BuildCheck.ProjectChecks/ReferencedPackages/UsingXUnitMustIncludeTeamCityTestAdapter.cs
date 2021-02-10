using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that the team city test adapter is installed for test projects.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class UsingXUnitMustIncludeTeamCityTestAdapter : HasAppropriateNonAnalysisPackages
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public UsingXUnitMustIncludeTeamCityTestAdapter(ILogger<UsingXUnitMustIncludeTeamCityTestAdapter> logger)
            : base(detectPackageId: @"xunit", mustIncludePackageId: @"TeamCity.VSTest.TestAdapter", logger: logger)
        {
        }
    }
}