namespace FunFair.BuildCheck.Interfaces;

public interface IFrameworkSettings
{
    public bool IsNullableGloballyEnforced { get; }

    string ProjectImport { get; }

    string? DotnetPackable { get; }

    string? DotnetPublishable { get; }

    string? DotnetTargetFramework { get; }

    string? DotNetSdkVersion { get; }

    string DotNetAllowPreReleaseSdk { get; }

    bool XmlDocumentationRequired { get; }
}
