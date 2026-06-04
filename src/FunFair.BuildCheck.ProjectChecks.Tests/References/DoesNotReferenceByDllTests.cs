using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.References;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.References;

public sealed class DoesNotReferenceByDllTests : TestBase
{
    [Theory]
    [InlineData("<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup></ItemGroup></Project>")]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Some.Package\" Version=\"1.0.0\" /></ItemGroup></Project>"
    )]
    public async Task WhenProjectHasNoDllReferencesNoErrorIsLoggedAsync(string xml)
    {
        XmlDocument doc = new();
        doc.LoadXml(xml);
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotReferenceByDll> logger = new();
        DoesNotReferenceByDll check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Theory]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><Reference Include=\"Some.Assembly\" /></ItemGroup></Project>",
        1
    )]
    [InlineData(
        "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><Reference Include=\"First.Assembly\" /><Reference Include=\"Second.Assembly\" /></ItemGroup></Project>",
        2
    )]
    public async Task WhenProjectReferencesByDllErrorsAreLoggedForEachAsync(string xml, int expectedErrorCount)
    {
        XmlDocument doc = new();
        doc.LoadXml(xml);
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<DoesNotReferenceByDll> logger = new();
        DoesNotReferenceByDll check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Equal(expected: expectedErrorCount, actual: logger.Entries.Count(e => e.Level == LogLevel.Error));
    }
}
