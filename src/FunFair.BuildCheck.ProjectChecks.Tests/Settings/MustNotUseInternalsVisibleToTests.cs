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

public sealed class MustNotUseInternalsVisibleToTests : TestBase
{
    [Fact]
    public async Task WhenProjectHasNoInternalsVisibleToNodesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup></ItemGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseInternalsVisibleTo> logger = new();
        MustNotUseInternalsVisibleTo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectHasOneInternalsVisibleToOneErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><InternalsVisibleTo Include=\"Other.Assembly\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseInternalsVisibleTo> logger = new();
        MustNotUseInternalsVisibleTo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenProjectHasTwoInternalsVisibleToTwoErrorsAreLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><InternalsVisibleTo Include=\"First.Assembly\" /><InternalsVisibleTo Include=\"Second.Assembly\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseInternalsVisibleTo> logger = new();
        MustNotUseInternalsVisibleTo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.Equal(expected: 2, actual: logger.Entries.Count(e => e.Level == LogLevel.Error));
    }

    [Fact]
    public async Task WhenProjectHasOnlyPackageReferencesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><PackageReference Include=\"Some.Package\" Version=\"1.0.0\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        CapturingLogger<MustNotUseInternalsVisibleTo> logger = new();
        MustNotUseInternalsVisibleTo check = new(logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }
}
