﻿using System;
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

    /// <inheritdoc />
    public string ProjectImport => Environment.GetEnvironmentVariable("DOTNET_PACK_PROJECT_METADATA_IMPORT") ?? string.Empty;

    /// <inheritdoc />
    public string DotnetPackable => Environment.GetEnvironmentVariable(variable: @"DOTNET_PACKABLE") ?? "NONE";

    /// <inheritdoc />
    public string DotnetPublishable => Environment.GetEnvironmentVariable(variable: @"DOTNET_PUBLISHABLE") ?? "NONE";

    /// <inheritdoc />
    public string? DotnetTargetFramework => Environment.GetEnvironmentVariable("DOTNET_CORE_APP_TARGET_FRAMEWORK");
}