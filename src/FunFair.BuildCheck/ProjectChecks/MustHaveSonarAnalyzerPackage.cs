using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that the C# Sonar Analyzer issues analyzer is installed.
    /// </summary>    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustHaveSonarAnalyzerPackage : MustHaveAnalyzerPackage
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public MustHaveSonarAnalyzerPackage(ILogger<MustHaveSonarAnalyzerPackage> logger)
            : base(packageId: @"SonarAnalyzer.CSharp", logger: logger)
        {
        }
    }
}
