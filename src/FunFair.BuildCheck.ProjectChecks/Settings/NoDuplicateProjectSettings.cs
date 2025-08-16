using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class NoDuplicateProjectSettings : IProjectCheck
{
    private readonly ILogger<NoDuplicateProjectSettings> _logger;

    public NoDuplicateProjectSettings(ILogger<NoDuplicateProjectSettings> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? properttyGroups = project.CsProjXml.SelectNodes("/Project/PropertyGroup");

        if (properttyGroups is null)
        {
            return ValueTask.CompletedTask;
        }

        Dictionary<string, NodeTracking> tracking = new(StringComparer.Ordinal);
        Dictionary<string, HashSet<string>> caseInsensitiveNames = new(StringComparer.OrdinalIgnoreCase);

        foreach (XmlElement propertyGroup in properttyGroups.OfType<XmlElement>())
        {
            if (propertyGroup.HasAttribute("Condition"))
            {
                // Skip property groups that have a condition
                continue;
            }

            foreach (XmlElement property in propertyGroup.ChildNodes.OfType<XmlElement>())
            {
                if (property.HasAttribute("Condition"))
                {
                    // Skip properties that have a condition
                    continue;
                }

                Include(tracking: tracking, caseInsensitiveNames: caseInsensitiveNames, node: property);
            }
        }

        foreach ((string propertyName, NodeTracking node) in tracking)
        {
            if (!node.IsUnique)
            {
                this._logger.PropertyAppearsMultipleTimes(project.Name, propertyName);
            }
        }

        foreach ((string propertyName, HashSet<string> names) in caseInsensitiveNames)
        {
            if (names.Count != 1)
            {
                this._logger.PropertyAppearsInMultipleCases(project.Name, propertyName, names);
            }
        }

        return ValueTask.CompletedTask;
    }

    private static NodeTracking GetNodeTracking(Dictionary<string, NodeTracking> tracking, XmlElement node)
    {
        if (tracking.TryGetValue(key: node.Name, out NodeTracking? nodeTracking))
        {
            return nodeTracking;
        }

        NodeTracking newItem = new();
        tracking.Add(key: node.Name, value: newItem);

        return newItem;
    }

    private static void Include(
        Dictionary<string, NodeTracking> tracking,
        Dictionary<string, HashSet<string>> caseInsensitiveNames,
        XmlElement node
    )
    {
        NodeTracking nt = GetNodeTracking(tracking: tracking, node: node);

        nt.Include(node);

        TrackCaseInsensitive(caseInsensitiveNames: caseInsensitiveNames, node: node);
    }

    private static void TrackCaseInsensitive(Dictionary<string, HashSet<string>> caseInsensitiveNames, XmlElement node)
    {
        if (caseInsensitiveNames.TryGetValue(key: node.Name, out HashSet<string>? items))
        {
            items.Add(node.Name);
        }
        else
        {
            HashSet<string> newItem = new(StringComparer.Ordinal) { node.Name };

            caseInsensitiveNames.Add(key: node.Name, value: newItem);
        }
    }

    private sealed class NodeTracking
    {
        private readonly List<(XmlElement node, bool includesReferenceToSelf)> _items;

        public NodeTracking()
        {
            this._items = [];
        }

        public bool IsUnique
        {
            get
            {
                if (this._items.Count == 1)
                {
                    return true;
                }

                return this._items.Where(item => !item.includesReferenceToSelf).Take(2).Count() == 1;
            }
        }

        public void Include(XmlElement node)
        {
            string value = node.InnerText.Trim();

            bool includesReferenceToSelf = value.Contains($"$({node.Name})", StringComparison.Ordinal);

            this._items.Add((node, includesReferenceToSelf));
        }
    }
}
