using System.Collections.Generic;
using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Runner;

internal sealed class RepositorySettings : IRepositorySettings
{
    private readonly IFrameworkSettings _frameworkSettings;
    private readonly IProjectClassifier _projectClassifier;
    private readonly IReadOnlyList<SolutionProject> _projects;

    public RepositorySettings(
        IFrameworkSettings frameworkSettings,
        IProjectClassifier projectClassifier,
        IReadOnlyList<SolutionProject> projects
    )
    {
        this._frameworkSettings = frameworkSettings;
        this._projectClassifier = projectClassifier;
        this._projects = projects;
    }

    public bool IsCodeAnalysisSolution => this._projectClassifier.IsCodeAnalysisSolution(this._projects);

    public bool IsNullableGloballyEnforced => this._frameworkSettings.IsNullableGloballyEnforced;

    public bool IsUnitTestBase => this._projectClassifier.IsUnitTestBase(this._projects);

    public string ProjectImport => this._frameworkSettings.ProjectImport;

    public string? DotnetPackable => this._frameworkSettings.DotnetPackable;

    public string? DotnetPublishable => this._frameworkSettings.DotnetPublishable;

    public string? DotnetTargetFramework => this._frameworkSettings.DotnetTargetFramework;

    public string? DotNetSdkVersion => this._frameworkSettings.DotNetSdkVersion;

    public string DotNetAllowPreReleaseSdk => this._frameworkSettings.DotNetAllowPreReleaseSdk;

    public bool XmlDocumentationRequired => this._frameworkSettings.XmlDocumentationRequired;

    public bool MustHaveEnumSourceGeneratorAnalyzerPackage =>
        this._projectClassifier.MustHaveEnumSourceGeneratorAnalyzerPackage(this._projects);
}
