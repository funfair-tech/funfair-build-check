using System;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.Tests.Settings;

public sealed class TestProjectsMustImportUnitTestsPropsTests : TestBase
{
    private const string UnitTestsImport = "$(SolutionDir)UnitTests.props";

    [Fact]
    public async Task TestProjectWithImportProducesNoErrorAsync()
    {
        CapturingLogger<TestProjectsMustImportUnitTestsProps> logger = new();
        TestProjectsMustImportUnitTestsProps check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tests.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="xunit" Version="2.9.0" />
              </ItemGroup>
              <Import Project="$(SolutionDir)UnitTests.props" Condition="Exists('$(SolutionDir)UnitTests.props')" />
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task TestProjectWithoutImportLogsErrorAsync()
    {
        CapturingLogger<TestProjectsMustImportUnitTestsProps> logger = new();
        TestProjectsMustImportUnitTestsProps check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Tests.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="xunit" Version="2.9.0" />
              </ItemGroup>
            </Project>
            """
        );

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        CapturedLogEntry entry = Assert.Single(collection: logger.Entries, predicate: e => e.Level == LogLevel.Error);
        Assert.Contains(
            expectedSubstring: UnitTestsImport,
            actualString: entry.Message,
            comparisonType: StringComparison.Ordinal
        );
        Assert.Contains(
            expectedSubstring: "Sample.Tests.csproj",
            actualString: entry.Message,
            comparisonType: StringComparison.Ordinal
        );
    }

    [Fact]
    public async Task NonTestProjectWithoutImportProducesNoErrorAsync()
    {
        CapturingLogger<TestProjectsMustImportUnitTestsProps> logger = new();
        TestProjectsMustImportUnitTestsProps check = new(logger);

        ProjectContext project = BuildProjectContext(
            name: "Sample.Library.csproj",
            csproj: """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="Newtonsoft.Json" Version="13.0.0" />
              </ItemGroup>
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
