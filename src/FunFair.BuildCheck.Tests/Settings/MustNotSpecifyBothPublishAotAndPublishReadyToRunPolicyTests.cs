using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.Tests.Settings;

public sealed class MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicyTests : TestBase
{
    [Fact]
    public async Task BothTrueLogsErrorAsync()
    {
        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <PublishAot>true</PublishAot>
                <PublishReadyToRun>true</PublishReadyToRun>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task AotTrueReadyToRunFalseProducesNoErrorAsync()
    {
        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <PublishAot>true</PublishAot>
                <PublishReadyToRun>false</PublishReadyToRun>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task AotFalseReadyToRunTrueProducesNoErrorAsync()
    {
        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <PublishAot>false</PublishAot>
                <PublishReadyToRun>true</PublishReadyToRun>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task BothFalseProducesNoErrorAsync()
    {
        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <PublishAot>false</PublishAot>
                <PublishReadyToRun>false</PublishReadyToRun>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task NeitherDefinedProducesNoErrorAsync()
    {
        CapturingLogger<MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy> logger = new();
        MustNotSpecifyBothPublishAotAndPublishReadyToRunPolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Exe.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
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
