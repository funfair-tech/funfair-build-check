using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.References;
using FunFair.BuildCheck.ProjectChecks.Tests.Helpers;
using FunFair.Test.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.References;

public sealed class PackableLibrariesShouldNotDependOnNonPackableTests : TestBase
{
    [Fact]
    public async Task WhenProjectIsNotLibraryNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
        CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
        PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenLibraryIsNotPackableNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
        CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
        PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackableLibraryHasNoProjectReferencesNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup></ItemGroup></Project>");
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
        CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
        PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackableLibraryReferencesNonExistentProjectNoErrorIsLoggedAsync()
    {
        XmlDocument doc = new();
        doc.LoadXml(
            "<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><ProjectReference Include=\"../NonExistent/NonExistent.csproj\" /></ItemGroup></Project>"
        );
        ProjectContext project = new(Name: "Test.csproj", Folder: "/test", CsProjXml: doc);

        IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
        CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
        PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

        await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

        Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
    }

    [Fact]
    public async Task WhenPackableLibraryReferencesPackableLibraryNoErrorIsLoggedAsync()
    {
        string tempFile = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(
                path: tempFile,
                contents: "<Project Sdk=\"Microsoft.NET.Sdk\" />",
                cancellationToken: this.CancellationToken()
            );
            string folder =
                Path.GetDirectoryName(tempFile) ?? throw new InvalidOperationException("Directory name is null");
            string fileName = Path.GetFileName(tempFile);

            XmlDocument doc = new();
            doc.LoadXml(
                $"<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><ProjectReference Include=\"{fileName}\" /></ItemGroup></Project>"
            );
            ProjectContext project = new(Name: "Test.csproj", Folder: folder, CsProjXml: doc);

            XmlDocument referencedDoc = new();
            referencedDoc.LoadXml("<Project Sdk=\"Microsoft.NET.Sdk\"></Project>");

            IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
            SubstituteExtensions.Returns(
                loader.LoadAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()),
                referencedDoc
            );

            CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
            PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

            await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

            Assert.DoesNotContain(collection: logger.Entries, filter: e => e.Level == LogLevel.Error);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task WhenPackableLibraryReferencesNonPackableLibraryErrorIsLoggedAsync()
    {
        string tempFile = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(
                path: tempFile,
                contents: "<Project Sdk=\"Microsoft.NET.Sdk\" />",
                cancellationToken: this.CancellationToken()
            );
            string folder =
                Path.GetDirectoryName(tempFile) ?? throw new InvalidOperationException("Directory name is null");
            string fileName = Path.GetFileName(tempFile);

            XmlDocument doc = new();
            doc.LoadXml(
                $"<Project Sdk=\"Microsoft.NET.Sdk\"><ItemGroup><ProjectReference Include=\"{fileName}\" /></ItemGroup></Project>"
            );
            ProjectContext project = new(Name: "Test.csproj", Folder: folder, CsProjXml: doc);

            XmlDocument referencedDoc = new();
            referencedDoc.LoadXml(
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><IsPackable>false</IsPackable></PropertyGroup></Project>"
            );

            IProjectXmlLoader loader = GetSubstitute<IProjectXmlLoader>();
            SubstituteExtensions.Returns(
                loader.LoadAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()),
                referencedDoc
            );

            CapturingLogger<PackableLibrariesShouldNotDependOnNonPackable> logger = new();
            PackableLibrariesShouldNotDependOnNonPackable check = new(projectXmlLoader: loader, logger: logger);

            await check.CheckAsync(project: project, cancellationToken: this.CancellationToken());

            Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
