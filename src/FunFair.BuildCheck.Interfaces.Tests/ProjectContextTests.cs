using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Interfaces.Tests;

public sealed class ProjectContextTests : TestBase
{
    [Fact]
    public void NameIsStoredCorrectly()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project />");

        ProjectContext context = new(Name: "MyProject.csproj", Folder: "/src/MyProject", CsProjXml: doc);

        Assert.Equal(expected: "MyProject.csproj", actual: context.Name);
    }

    [Fact]
    public void FolderIsStoredCorrectly()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project />");

        ProjectContext context = new(Name: "MyProject.csproj", Folder: "/src/MyProject", CsProjXml: doc);

        Assert.Equal(expected: "/src/MyProject", actual: context.Folder);
    }

    [Fact]
    public void CsProjXmlIsStoredCorrectly()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project />");

        ProjectContext context = new(Name: "MyProject.csproj", Folder: "/src/MyProject", CsProjXml: doc);

        Assert.Same(expected: doc, actual: context.CsProjXml);
    }

    [Fact]
    public void EqualityHoldsForIdenticalValues()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project />");

        ProjectContext a = new(Name: "MyProject.csproj", Folder: "/src/MyProject", CsProjXml: doc);
        ProjectContext b = new(Name: "MyProject.csproj", Folder: "/src/MyProject", CsProjXml: doc);

        Assert.Equal(expected: a, actual: b);
    }

    [Fact]
    public void InequalityHoldsWhenNameDiffers()
    {
        XmlDocument doc = new();
        doc.LoadXml("<Project />");

        ProjectContext a = new(Name: "A.csproj", Folder: "/src", CsProjXml: doc);
        ProjectContext b = new(Name: "B.csproj", Folder: "/src", CsProjXml: doc);

        Assert.NotEqual(a, b);
    }
}
