using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.ReferencedPackages;

public sealed class UsingPackageTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // UsingJwtAuthenticationMustIncludeIdentityModelJwt (MustHaveRelatedPackage)
    // detect: Microsoft.AspNetCore.Authentication.JwtBearer
    // mustInclude: System.IdentityModel.Tokens.Jwt
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenJwtAuthAndIdentityModelBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.AspNetCore.Authentication.JwtBearer\" Version=\"8.0.0\" /><PackageReference Include=\"System.IdentityModel.Tokens.Jwt\" Version=\"7.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingJwtAuthenticationMustIncludeIdentityModelJwt> logger = new();
        UsingJwtAuthenticationMustIncludeIdentityModelJwt check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenJwtAuthPresentButIdentityModelMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Microsoft.AspNetCore.Authentication.JwtBearer\" Version=\"8.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingJwtAuthenticationMustIncludeIdentityModelJwt> logger = new();
        UsingJwtAuthenticationMustIncludeIdentityModelJwt check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenJwtAuthNotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingJwtAuthenticationMustIncludeIdentityModelJwt> logger = new();
        UsingJwtAuthenticationMustIncludeIdentityModelJwt check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // UsingXUnitV2MustIncludeAnalyzer (HasAppropriateAnalysisPackages)
    // detect: xunit / mustInclude: xunit.analyzers
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV2AndAnalyzerBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.9.0\" /><PackageReference Include=\"xunit.analyzers\" Version=\"1.0.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV2MustIncludeAnalyzer> logger = new();
        UsingXUnitV2MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV2PresentButAnalyzerMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit\" Version=\"2.9.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV2MustIncludeAnalyzer> logger = new();
        UsingXUnitV2MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV2NotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV2MustIncludeAnalyzer> logger = new();
        UsingXUnitV2MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // UsingXUnitV3MustIncludeAnalyzer (HasAppropriateAnalysisPackages)
    // detect: xunit.v3 / mustInclude: xunit.analyzers
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3AndAnalyzerBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"0.4.0\" /><PackageReference Include=\"xunit.analyzers\" Version=\"1.0.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3MustIncludeAnalyzer> logger = new();
        UsingXUnitV3MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3PresentButAnalyzerMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit.v3\" Version=\"0.4.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3MustIncludeAnalyzer> logger = new();
        UsingXUnitV3MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3NotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3MustIncludeAnalyzer> logger = new();
        UsingXUnitV3MustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // UsingXUnitV3ExtensibilityMustIncludeAnalyzer (HasAppropriateAnalysisPackages)
    // detect: xunit.v3.extensibility.core / mustInclude: xunit.analyzers
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenXUnitV3ExtensibilityAndAnalyzerBothPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit.v3.extensibility.core\" Version=\"0.4.0\" /><PackageReference Include=\"xunit.analyzers\" Version=\"1.0.0\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3ExtensibilityMustIncludeAnalyzer> logger = new();
        UsingXUnitV3ExtensibilityMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ExtensibilityPresentButAnalyzerMissingErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"xunit.v3.extensibility.core\" Version=\"0.4.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3ExtensibilityMustIncludeAnalyzer> logger = new();
        UsingXUnitV3ExtensibilityMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenXUnitV3ExtensibilityNotPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"SomeOtherPackage\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<UsingXUnitV3ExtensibilityMustIncludeAnalyzer> logger = new();
        UsingXUnitV3ExtensibilityMustIncludeAnalyzer check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
