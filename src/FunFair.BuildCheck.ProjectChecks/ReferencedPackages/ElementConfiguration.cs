using System.Xml;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

internal static class ElementConfiguration
{
    public static bool HasNoParentCondition(XmlElement item)
    {
        if (item.ParentNode is not XmlElement propertyGroup)
        {
            return false;
        }

        string condition = propertyGroup.GetAttribute(name: "Condition");

        return string.IsNullOrWhiteSpace(condition);
    }
}