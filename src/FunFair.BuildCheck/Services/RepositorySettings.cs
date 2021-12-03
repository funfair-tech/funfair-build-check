using System;
using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Services;

internal sealed class RepositorySettings : IRepositorySettings
{
    /// <inheritdoc />
    public bool IsCodeAnalysisSolution
    {
        get
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"BUILD_CODEANALYSIS");

            return !string.IsNullOrWhiteSpace(codeAnalysis) && StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }
    }

    /// <inheritdoc />
    public bool IsNullableGloballyEnforced
    {
        get
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"DISABLE_BUILD_NULLABLE_REFERENCE_TYPES");

            return string.IsNullOrWhiteSpace(codeAnalysis) || !StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }
    }

    /// <inheritdoc />
    public bool IsUnitTestBase
    {
        get
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: @"IS_UNITTEST_BASE");

            return !string.IsNullOrWhiteSpace(codeAnalysis) && StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: @"TRUE");
        }
    }
}