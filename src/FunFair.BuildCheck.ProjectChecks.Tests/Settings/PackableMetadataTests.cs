using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class PackableMetadataTests : TestBase
{
    // ──────────────────────────────────────────────────────────────
    // DescriptionMetadata  (property="Description")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenDescriptionMetadataProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DescriptionMetadata> logger = new();
        DescriptionMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDescriptionMetadataProjectIsPackableAndPropertyIsPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><Description>My package description.</Description></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DescriptionMetadata> logger = new();
        DescriptionMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenDescriptionMetadataProjectIsPackableAndPropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DescriptionMetadata> logger = new();
        DescriptionMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // PackageTagsMetadata  (property="PackageTags")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenPackageTagsMetadataProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PackageTagsMetadata> logger = new();
        PackageTagsMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageTagsMetadataProjectIsPackableAndPropertyIsPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><PackageTags>dotnet;library</PackageTags></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PackageTagsMetadata> logger = new();
        PackageTagsMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageTagsMetadataProjectIsPackableAndPropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<PackageTagsMetadata> logger = new();
        PackageTagsMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // RepositoryUrlMetadata  (property="RepositoryUrl")
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenRepositoryUrlMetadataProjectIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<RepositoryUrlMetadata> logger = new();
        RepositoryUrlMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenRepositoryUrlMetadataProjectIsPackableAndPropertyIsPresentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><RepositoryUrl>https://github.com/example/repo</RepositoryUrl></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<RepositoryUrlMetadata> logger = new();
        RepositoryUrlMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenRepositoryUrlMetadataProjectIsPackableAndPropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<RepositoryUrlMetadata> logger = new();
        RepositoryUrlMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    // ──────────────────────────────────────────────────────────────
    // WarnOnPackingNonPackableProjectMetadata
    // Non-packable project without WarnOnPackingNonPackableProject → error
    // Non-packable project with WarnOnPackingNonPackableProject=false → no error
    // Packable project with WarnOnPackingNonPackableProject defined → error
    // Packable project without WarnOnPackingNonPackableProject → no error
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task WhenWarnOnPackingNonPackableProjectMetadataProjectIsNotPackableAndPropertyIsAbsentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<WarnOnPackingNonPackableProjectMetadata> logger = new();
        WarnOnPackingNonPackableProjectMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenWarnOnPackingNonPackableProjectMetadataProjectIsNotPackableAndPropertyIsFalseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable><WarnOnPackingNonPackableProject>false</WarnOnPackingNonPackableProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<WarnOnPackingNonPackableProjectMetadata> logger = new();
        WarnOnPackingNonPackableProjectMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenWarnOnPackingNonPackableProjectMetadataProjectIsPackableAndPropertyIsAbsentNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup></PropertyGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<WarnOnPackingNonPackableProjectMetadata> logger = new();
        WarnOnPackingNonPackableProjectMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenWarnOnPackingNonPackableProjectMetadataProjectIsPackableAndPropertyIsPresentErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><WarnOnPackingNonPackableProject>true</WarnOnPackingNonPackableProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<WarnOnPackingNonPackableProjectMetadata> logger = new();
        WarnOnPackingNonPackableProjectMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenWarnOnPackingNonPackableProjectMetadataProjectIsNotPackableAndPropertyIsNotFalseErrorIsLoggedAsync()
    {
        // Non-packable project with WarnOnPackingNonPackableProject set but NOT to "false" → error
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable><WarnOnPackingNonPackableProject>true</WarnOnPackingNonPackableProject></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<WarnOnPackingNonPackableProjectMetadata> logger = new();
        WarnOnPackingNonPackableProjectMetadata check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
