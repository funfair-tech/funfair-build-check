using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace FunFair.BuildCheck.ProjectChecks.Models;

[DebuggerDisplay("{Id}")]
public readonly record struct PackageReference(
    string Id,
    IReadOnlyDictionary<string, string> Attributes,
    XmlElement Element
)
{
    public string? Version => this.GetAttribute("Version");

    public string? GetAttribute(string attibuteName)
    {
        return this.Attributes.TryGetValue(key: attibuteName, out string? value) && !string.IsNullOrEmpty(value)
            ? value
            : null;
    }

    public string? GetAttributeOrElement(string attributeOrElementName)
    {
        // check for an attribute
        if (this.Attributes.TryGetValue(key: attributeOrElementName, out string? value) && !string.IsNullOrEmpty(value))
        {
            return value;
        }

        // no attribute, check for an element
        if (this.Element.SelectSingleNode(xpath: attributeOrElementName) is XmlElement privateAssets)
        {
            return privateAssets.InnerText;
        }

        return null;
    }
}
