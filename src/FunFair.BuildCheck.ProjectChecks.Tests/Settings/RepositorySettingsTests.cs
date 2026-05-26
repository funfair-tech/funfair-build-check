using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class RepositorySettingsTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // ImportCommonProps
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenImportCommonPropsProjectImportIsEmptyNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.ProjectImport.Returns(string.Empty);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ImportCommonProps> logger = new();
        ImportCommonProps check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenImportCommonPropsProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.ProjectImport.Returns("$(SolutionDir)common.props");

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ImportCommonProps> logger = new();
        ImportCommonProps check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenImportCommonPropsPackableProjectHasImportNoErrorIsLoggedAsync()
    {
        const string importPath = "$(SolutionDir)common.props";
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.ProjectImport.Returns(importPath);

        XmlDocument doc = new();
        doc.LoadXml($"<Project Sdk=\"Microsoft.NET.Sdk\"><Import Project=\"{importPath}\" /></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ImportCommonProps> logger = new();
        ImportCommonProps check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenImportCommonPropsPackableProjectMissingImportErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.ProjectImport.Returns("$(SolutionDir)common.props");

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<ImportCommonProps> logger = new();
        ImportCommonProps check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // TargetFrameworkIsSetCorrectlyPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenTargetFrameworkIsSetCorrectlyPolicyNoTargetFrameworkConfiguredNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.DotnetTargetFramework.Returns((string?)null);
        repositorySettings.IsCodeAnalysisSolution.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TargetFrameworkIsSetCorrectlyPolicy> logger = new();
        TargetFrameworkIsSetCorrectlyPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTargetFrameworkIsSetCorrectlyPolicySingleFrameworkMatchesNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.DotnetTargetFramework.Returns("net10.0");
        repositorySettings.IsCodeAnalysisSolution.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TargetFrameworkIsSetCorrectlyPolicy> logger = new();
        TargetFrameworkIsSetCorrectlyPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTargetFrameworkIsSetCorrectlyPolicySingleFrameworkMismatchErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.DotnetTargetFramework.Returns("net10.0");
        repositorySettings.IsCodeAnalysisSolution.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TargetFrameworkIsSetCorrectlyPolicy> logger = new();
        TargetFrameworkIsSetCorrectlyPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTargetFrameworkIsSetCorrectlyPolicyMultipleFrameworksMatchNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.DotnetTargetFramework.Returns("net9.0;net10.0");
        repositorySettings.IsCodeAnalysisSolution.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFrameworks>net9.0;net10.0</TargetFrameworks></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TargetFrameworkIsSetCorrectlyPolicy> logger = new();
        TargetFrameworkIsSetCorrectlyPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenTargetFrameworkIsSetCorrectlyPolicyCodeAnalysisSolutionNonTestProjectRequiresNetStandardNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.DotnetTargetFramework.Returns("net10.0");
        repositorySettings.IsCodeAnalysisSolution.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>netstandard2.0</TargetFramework></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<TargetFrameworkIsSetCorrectlyPolicy> logger = new();
        TargetFrameworkIsSetCorrectlyPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XmlDocumentationFileRequiredPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXmlDocumentationFileRequiredPolicyNotRequiredNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(false);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileRequiredPolicy> logger = new();
        XmlDocumentationFileRequiredPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXmlDocumentationFileRequiredPolicyRequiredAndDocFileCorrectNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(true);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml</DocumentationFile></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileRequiredPolicy> logger = new();
        XmlDocumentationFileRequiredPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXmlDocumentationFileRequiredPolicyRequiredAndDocFileMissingErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(true);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileRequiredPolicy> logger = new();
        XmlDocumentationFileRequiredPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXmlDocumentationFileRequiredPolicyTestProjectHasDocFileErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(true);
        repositorySettings.IsUnitTestBase.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml</DocumentationFile></PropertyGroup><ItemGroup><PackageReference Include=""xunit.v3"" Version=""0.4.0"" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileRequiredPolicy> logger = new();
        XmlDocumentationFileRequiredPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // XmlDocumentationFileProhibitedPolicy
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXmlDocumentationFileProhibitedPolicyDocumentationIsRequiredNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(true);

        XmlDocument doc = new();
        doc.LoadXml(
            @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml</DocumentationFile></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileProhibitedPolicy> logger = new();
        XmlDocumentationFileProhibitedPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXmlDocumentationFileProhibitedPolicyDocumentationIsNotRequiredAndNoDocFileNoErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileProhibitedPolicy> logger = new();
        XmlDocumentationFileProhibitedPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXmlDocumentationFileProhibitedPolicyDocumentationIsNotRequiredButDocFileExistsErrorIsLoggedAsync()
    {
        IRepositorySettings repositorySettings = Substitute.For<IRepositorySettings>();
        repositorySettings.XmlDocumentationRequired.Returns(false);

        XmlDocument doc = new();
        doc.LoadXml(
            @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml</DocumentationFile></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<XmlDocumentationFileProhibitedPolicy> logger = new();
        XmlDocumentationFileProhibitedPolicy check = new(repositorySettings: repositorySettings, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
