using System.IO;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Runner.Services;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Runner.Tests.Services;

public sealed class ProjectXmlLoaderTests : TestBase
{
    [Fact]
    public async Task LoadAsyncLoadsValidXmlDocumentAsync()
    {
        string tempFile = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(tempFile, "<Project />", this.CancellationToken());

            ProjectXmlLoader loader = new();
            XmlDocument doc = await loader.LoadAsync(path: tempFile, cancellationToken: this.CancellationToken());

            Assert.NotNull(doc);
            Assert.Equal(expected: "Project", actual: doc.DocumentElement?.Name);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task LoadAsyncReturnsCachedDocumentForSamePathAsync()
    {
        string tempFile = Path.GetTempFileName();

        try
        {
            await File.WriteAllTextAsync(tempFile, "<Project />", this.CancellationToken());

            ProjectXmlLoader loader = new();
            XmlDocument first = await loader.LoadAsync(path: tempFile, cancellationToken: this.CancellationToken());
            XmlDocument second = await loader.LoadAsync(path: tempFile, cancellationToken: this.CancellationToken());

            Assert.Same(expected: first, actual: second);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
