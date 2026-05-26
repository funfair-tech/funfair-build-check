using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.BuildCheck.Runner.Services;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class NoPreReleaseNuGetPackagesTests : TestBase
{
    [Fact]
    public async Task WhenProjectHasNoPackagesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\" />");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageHasReleaseVersionNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageHasPreReleaseVersionAndBuildIsNotPreReleaseErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0-beta.1\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageHasPreReleaseVersionAndBuildIsPreReleaseNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0-beta.1\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: true, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageHasPrivateAssetsAllItIsSkippedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" Version=\"1.0.0-beta.1\" PrivateAssets=\"All\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackageHasNoVersionErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Foo\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<NoPreReleaseNuGetPackages> logger = new();
        CheckConfiguration configuration = new(preReleaseBuild: false, allowPackageVersionMismatch: false);
        NoPreReleaseNuGetPackages check = new(configuration: configuration, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }
}
