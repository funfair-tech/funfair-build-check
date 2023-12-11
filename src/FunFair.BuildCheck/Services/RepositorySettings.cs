using System;
using System.Collections.Generic;
using System.Linq;
using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Services;

internal sealed class RepositorySettings : IRepositorySettings
{
    private readonly IReadOnlyList<SolutionProject> _projects;

    public RepositorySettings(IReadOnlyList<SolutionProject> projects)
    {
        this._projects = projects;
    }

    public bool IsCodeAnalysisSolution => this.HasNamedProject("FunFair.CodeAnalysis");

    public bool IsNullableGloballyEnforced
    {
        get
        {
            string? codeAnalysis = Environment.GetEnvironmentVariable(variable: "DISABLE_BUILD_NULLABLE_REFERENCE_TYPES");

            return string.IsNullOrWhiteSpace(codeAnalysis) || !StringComparer.InvariantCultureIgnoreCase.Equals(x: codeAnalysis, y: "TRUE");
        }
    }

    public bool IsUnitTestBase => this.HasNamedProject("FunFair.Test.Common");

    public string ProjectImport => Environment.GetEnvironmentVariable("DOTNET_PACK_PROJECT_METADATA_IMPORT") ?? string.Empty;

    public string? DotnetPackable => Environment.GetEnvironmentVariable(variable: "DOTNET_PACKABLE");

    public string? DotnetPublishable => Environment.GetEnvironmentVariable(variable: "DOTNET_PUBLISHABLE");

    public string? DotnetTargetFramework => Environment.GetEnvironmentVariable("DOTNET_CORE_APP_TARGET_FRAMEWORK");

    public string? DotNetSdkVersion => Environment.GetEnvironmentVariable(variable: "DOTNET_CORE_SDK_VERSION");

    public string DotNetAllowPreReleaseSdk => Environment.GetEnvironmentVariable("DOTNET_SDK_ALLOW_PRE_RELEASE") ?? "false";

    public bool XmlDocumentationRequired => StringComparer.InvariantCulture.Equals(Environment.GetEnvironmentVariable("XML_DOCUMENTATION"), y: "true");

    public bool MustHaveEnumSourceGeneratorAnalyzerPackage => this.HasNamedProject("Credfeto.Enumeration.Source.Generation");

    private bool HasNamedProject(string projectName)
    {
        return this._projects.Any(project => StringComparer.InvariantCultureIgnoreCase.Equals(x: project.DisplayName, y: projectName));
    }
}