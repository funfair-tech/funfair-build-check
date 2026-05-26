using System.Collections.Generic;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Models;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Models;

public sealed class PackageReferenceTests : TestBase
{
    [Fact]
    public void VersionReturnsCorrectValueWhenPresent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");
        elem.SetAttribute("Version", "1.0.0");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
            ["Version"] = "1.0.0",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Equal(expected: "1.0.0", actual: package.Version);
    }

    [Fact]
    public void VersionReturnsNullWhenAbsent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Null(package.Version);
    }

    [Fact]
    public void GetAttributeReturnsValueWhenPresent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");
        elem.SetAttribute("PrivateAssets", "All");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
            ["PrivateAssets"] = "All",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Equal(expected: "All", actual: package.GetAttribute("PrivateAssets"));
    }

    [Fact]
    public void GetAttributeReturnsNullWhenAbsent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Null(package.GetAttribute("PrivateAssets"));
    }

    [Fact]
    public void GetAttributeOrElementReturnsAttributeValueWhenPresent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");
        elem.SetAttribute("PrivateAssets", "All");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
            ["PrivateAssets"] = "All",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Equal(expected: "All", actual: package.GetAttributeOrElement("PrivateAssets"));
    }

    [Fact]
    public void GetAttributeOrElementReturnsElementValueWhenAttributeAbsent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");
        XmlElement child = xmlDoc.CreateElement("PrivateAssets");
        child.InnerText = "All";
        elem.AppendChild(child);

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Equal(expected: "All", actual: package.GetAttributeOrElement("PrivateAssets"));
    }

    [Fact]
    public void GetAttributeOrElementReturnsNullWhenNeitherPresent()
    {
        XmlDocument xmlDoc = new();
        XmlElement elem = xmlDoc.CreateElement("PackageReference");
        elem.SetAttribute("Include", "Foo");

        IReadOnlyDictionary<string, string> attributes = new Dictionary<string, string>(System.StringComparer.Ordinal)
        {
            ["Include"] = "Foo",
        };

        PackageReference package = new(Id: "Foo", Attributes: attributes, Element: elem);

        Assert.Null(package.GetAttributeOrElement("PrivateAssets"));
    }
}
