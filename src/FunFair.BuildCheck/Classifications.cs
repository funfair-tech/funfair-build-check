using System;

namespace FunFair.BuildCheck
{
    internal static class Classifications
    {
        public static bool IsCodeAnalysisSolution()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"BUILD_CODEANALYSIS");

            return !string.IsNullOrWhiteSpace(codeAnalysis) && StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }

        public static bool IsNullableGloballyEnforced()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"DISABLE_BUILD_NULLABLE_REFERENCE_TYPES");

            return string.IsNullOrWhiteSpace(codeAnalysis) || !StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }

        public static bool IsUnitTestBase()
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"IS_UNITTEST_BASE");

            return !string.IsNullOrWhiteSpace(codeAnalysis) && StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }
    }
}