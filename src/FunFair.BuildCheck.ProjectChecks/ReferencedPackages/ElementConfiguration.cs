using System.Xml;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

internal static class ElementConfiguration
{
    public static bool HasNoParentCondition(XmlElement item)
    {
        return item.ParentNode is XmlElement propertyGroup
            && string.IsNullOrWhiteSpace(propertyGroup.GetAttribute(name: "Condition"));
    }
}
