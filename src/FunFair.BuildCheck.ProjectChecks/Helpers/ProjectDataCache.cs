using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

namespace FunFair.BuildCheck.ProjectChecks.Helpers;

internal static class ProjectDataCache
{
    private static readonly ConditionalWeakTable<XmlDocument, CachedProjectData> Cache = [];

    public static CachedProjectData Get(XmlDocument doc)
    {
        return Cache.GetValue(doc, static d => new(d));
    }

    internal sealed class CachedProjectData
    {
        public CachedProjectData(XmlDocument doc)
        {
            this.ConditionalPropertyGroups =
            [
                .. doc.SelectNodes("/Project/PropertyGroup[@Condition]")?.OfType<XmlElement>() ?? [],
            ];
            this.PackageReferenceNodes =
            [
                .. doc.SelectNodes("/Project/ItemGroup/PackageReference")?.OfType<XmlElement>() ?? [],
            ];
            this.ImportNodes = [.. doc.SelectNodes("/Project/Import[@Project]")?.OfType<XmlElement>() ?? []];
        }

        public IReadOnlyList<XmlElement> ConditionalPropertyGroups { get; }

        public IReadOnlyList<XmlElement> PackageReferenceNodes { get; }

        public IReadOnlyList<XmlElement> ImportNodes { get; }
    }
}
