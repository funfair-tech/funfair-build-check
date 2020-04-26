using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class UsingXUnitMustIncludeVisualStudioTestPlatform : HasAppropriateNonAnalysisPackages
    {
        public UsingXUnitMustIncludeVisualStudioTestPlatform(ILogger<NoPreReleaseNuGetPackages> logger)
            : base(detectPackageId: @"xunit", mustIncludePackageId: @"Microsoft.NET.Test.Sdk", logger)
        {
        }
    }
}