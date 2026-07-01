using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Settings;

public sealed class MustNotUseAssemblyAttributeItemsTests : TestBase
{
    [Theory]
    [InlineData("<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup></ItemGroup></Project>")]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Some.Package\" Version=\"1.0.0\" /></ItemGroup></Project>"
    )]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><InternalsVisibleTo Include=\"Other.Assembly\" /></ItemGroup></Project>"
    )]
    public async Task WhenProjectHasNoAssemblyAttributeItemsNoErrorIsLoggedAsync(string xml)
    {
        XmlDocument doc = new();
        doc.LoadXml(xml);
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseAssemblyAttributeItems> logger = new();
        MustNotUseAssemblyAttributeItems check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Theory]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><AssemblyAttribute Include=\"System.Runtime.CompilerServices.InternalsVisibleToAttribute\"><_Parameter1>FunFair.Common.Server.Tests</_Parameter1></AssemblyAttribute></ItemGroup></Project>",
        1
    )]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><AssemblyAttribute Include=\"System.Runtime.CompilerServices.InternalsVisibleToAttribute\"><_Parameter1>First.Assembly</_Parameter1></AssemblyAttribute><AssemblyAttribute Include=\"System.Reflection.AssemblyMetadataAttribute\"><_Parameter1>Key</_Parameter1><_Parameter2>Value</_Parameter2></AssemblyAttribute></ItemGroup></Project>",
        2
    )]
    public async Task WhenProjectHasAssemblyAttributeItemsErrorsAreLoggedForEachAsync(
        string xml,
        int expectedErrorCount
    )
    {
        XmlDocument doc = new();
        doc.LoadXml(xml);
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseAssemblyAttributeItems> logger = new();
        MustNotUseAssemblyAttributeItems check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Equal(expected: expectedErrorCount, actual: logger.Entries.Count(e => e.Level == LogLevel.Error));
    }
}
