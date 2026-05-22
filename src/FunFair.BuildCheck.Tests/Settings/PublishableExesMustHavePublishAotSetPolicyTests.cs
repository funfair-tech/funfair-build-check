using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.Tests.Settings;

public sealed class PublishableExesMustHavePublishAotSetPolicyTests : TestBase
{
    [Fact]
    public async Task PublishableExeWithPublishAotTrueProducesNoErrorAsync()
    {
        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <IsPublishable>true</IsPublishable>
                <PublishAot>true</PublishAot>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task PublishableExeWithPublishAotFalseProducesNoErrorAsync()
    {
        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <IsPublishable>true</IsPublishable>
                <PublishAot>false</PublishAot>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task PublishableExeWithoutPublishAotLogsErrorAsync()
    {
        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <IsPublishable>true</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task NonPublishableExeWithoutPublishAotProducesNoErrorAsync()
    {
        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task LibraryWithoutPublishAotProducesNoErrorAsync()
    {
        CapturingLogger<PublishableExesMustHavePublishAotSetPolicy> logger = new();
        PublishableExesMustHavePublishAotSetPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Library.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Library</OutputType>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    private static ProjectContext BuildProjectContext(string name, string csproj)
    {
        XmlDocument doc = new();
        doc.LoadXml(csproj);

        return new(Name: name, Folder: "/tmp", CsProjXml: doc);
    }
}
