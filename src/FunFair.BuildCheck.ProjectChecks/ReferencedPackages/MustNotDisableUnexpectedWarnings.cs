using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotDisableUnexpectedWarnings : IProjectCheck
{
    private static readonly IReadOnlyList<string> AllowedWarnings = new[]
                                                                    {
                                                                        // Xml Docs
                                                                        "1591"
                                                                    };

    private static readonly IReadOnlyList<string> AllowedTestProjectWarnings = Array.Empty<string>();

    private readonly ILogger<MustNotDisableUnexpectedWarnings> _logger;

    public MustNotDisableUnexpectedWarnings(ILogger<MustNotDisableUnexpectedWarnings> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        bool isTestProject = project.IsTestProject(projectName: projectName, logger: this._logger);

        IReadOnlyList<string> allowedWarnings = isTestProject
            ? AllowedTestProjectWarnings
            : AllowedWarnings;

        const string nodePresence = @"NoWarn";
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);

        if (nodes != null)
        {
            this.CheckGlobalConfiguration(projectName: projectName, nodes: nodes, allowedWarnings: allowedWarnings);
        }

        XmlNodeList? configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

        if (configurationGroups != null)
        {
            this.CheckConfigurationGroup(projectName: projectName, configurationGroups: configurationGroups, nodePresence: nodePresence, allowedWarnings: allowedWarnings);
        }
    }

    private void CheckConfigurationGroup(string projectName, XmlNodeList configurationGroups, string nodePresence, IReadOnlyList<string> allowedWarnings)
    {
        foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
        {
            XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

            if (node == null)
            {
                continue;
            }

            string value = GetTextValue(node);

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            string configuration = propertyGroup.GetAttribute(name: "Condition");

            IReadOnlyList<string> warnings = ExtractWarnings(value);

            foreach (string warning in warnings)
            {
                if (warning == "$(NoWarn)")
                {
                    // skip references to global configs
                    continue;
                }

                if (!allowedWarnings.Contains(value: warning, comparer: StringComparer.OrdinalIgnoreCase))
                {
                    this._logger.LogError($"{projectName}: Configuration {configuration} hides warning {warning}.");
                }
            }
        }
    }

    private void CheckGlobalConfiguration(string projectName, XmlNodeList nodes, IReadOnlyList<string> allowedWarnings)
    {
        foreach (XmlElement item in nodes.OfType<XmlElement>())
        {
            if (item.ParentNode is not XmlElement propertyGroup)
            {
                continue;
            }

            string condition = propertyGroup.GetAttribute(name: "Condition");

            if (!string.IsNullOrWhiteSpace(condition))
            {
                continue;
            }

            string value = GetTextValue(item);

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            IReadOnlyList<string> warnings = ExtractWarnings(value);

            foreach (string warning in warnings)
            {
                if (!allowedWarnings.Contains(value: warning, comparer: StringComparer.OrdinalIgnoreCase))
                {
                    this._logger.LogError($"{projectName}: Global Configuration hides warning {warning}.");
                }
            }
        }
    }

    private static IReadOnlyList<string> ExtractWarnings(string value)
    {
        return value.Split(separator: ";")
                    .Where(predicate: HasContent)
                    .SelectMany(selector: s => s.Split(separator: ",")
                                                .Where(predicate: HasContent))
                    .Where(predicate: s => !string.IsNullOrWhiteSpace(s))
                    .OrderBy(keySelector: s => s, comparer: StringComparer.OrdinalIgnoreCase)
                    .ToArray();
    }

    private static bool HasContent(string t)
    {
        return !string.IsNullOrWhiteSpace(t);
    }

    private static string GetTextValue(XmlNode node)
    {
        return node.InnerText;
    }
}