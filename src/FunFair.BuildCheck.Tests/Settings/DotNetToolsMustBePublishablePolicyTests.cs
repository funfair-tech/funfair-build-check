using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.Tests.Settings;

public sealed class DotNetToolsMustBePublishablePolicyTests : TestBase
{
    [Fact]
    public async Task ToolWithPackAsToolTrueAndIsPublishableTrueProducesNoErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <PackAsTool>true</PackAsTool>
                <IsPublishable>true</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task ToolWithPackAsToolTrueAndNoIsPublishableProducesNoErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <PackAsTool>true</PackAsTool>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task ToolWithPackAsToolTrueAndIsPublishableFalseLogsErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <PackAsTool>true</PackAsTool>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task ToolWithIsToolTrueAndIsPublishableFalseLogsErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <IsTool>true</IsTool>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task ToolWithToolCommandNameSetAndIsPublishableFalseLogsErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <ToolCommandName>mytool</ToolCommandName>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task ToolWithToolCommandNameSetAndIsPublishableTrueProducesNoErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tool.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <ToolCommandName>mytool</ToolCommandName>
                <IsPublishable>true</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task NonToolLibraryWithIsPublishableFalseProducesNoErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Library.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Library</OutputType>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task NonToolExeWithIsPublishableFalseProducesNoErrorAsync()
    {
        CapturingLogger<DotNetToolsMustBePublishablePolicy> logger = new();
        DotNetToolsMustBePublishablePolicy check = new(logger);

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

    private static ProjectContext BuildProjectContext(string name, string csproj)
    {
        XmlDocument doc = new();
        doc.LoadXml(csproj);

        return new(Name: name, Folder: "/tmp", CsProjXml: doc);
    }
}
