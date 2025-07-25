using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotDisableUnexpectedWarnings : IProjectCheck
{
    private static readonly IReadOnlyList<string> AllowedWarnings =
    [
        // Xml Docs
        "1591",
    ];

    private static readonly IReadOnlyList<string> AllowedTestProjectWarnings = [];

    private readonly ILogger<MustNotDisableUnexpectedWarnings> _logger;

    public MustNotDisableUnexpectedWarnings(ILogger<MustNotDisableUnexpectedWarnings> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        bool isTestProject = project.IsTestProject(logger: this._logger);

        IReadOnlyList<string> allowedWarnings = isTestProject ? AllowedTestProjectWarnings : AllowedWarnings;

        const string nodePresence = "NoWarn";
        XmlNodeList? nodes = project.CsProjXml.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);

        if (nodes is not null)
        {
            this.CheckGlobalConfiguration(projectName: project.Name, nodes: nodes, allowedWarnings: allowedWarnings);
        }

        XmlNodeList? configurationGroups = project.CsProjXml.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

        if (configurationGroups is not null)
        {
            this.CheckConfigurationGroup(
                projectName: project.Name,
                configurationGroups: configurationGroups,
                nodePresence: nodePresence,
                allowedWarnings: allowedWarnings
            );
        }

        return ValueTask.CompletedTask;
    }

    private void CheckConfigurationGroup(
        string projectName,
        XmlNodeList configurationGroups,
        string nodePresence,
        IReadOnlyList<string> allowedWarnings
    )
    {
        foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
        {
            XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

            if (node is null)
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

            foreach (
                string warning in warnings.Where(warning =>
                    !ReferencesGlobalWarning(warning)
                    && !allowedWarnings.Contains(value: warning, comparer: StringComparer.OrdinalIgnoreCase)
                )
            )
            {
                this._logger.ConfigurationHidesWarning(
                    projectName: projectName,
                    configuration: configuration,
                    warning: warning
                );
            }
        }
    }

    private static bool ReferencesGlobalWarning(string warning)
    {
        return StringComparer.Ordinal.Equals(x: warning, y: "$(NoWarn)");
    }

    private void CheckGlobalConfiguration(string projectName, XmlNodeList nodes, IReadOnlyList<string> allowedWarnings)
    {
        foreach (XmlElement item in nodes.OfType<XmlElement>().Where(ElementConfiguration.HasNoParentCondition))
        {
            string value = GetTextValue(item);

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            IReadOnlyList<string> warnings = ExtractWarnings(value);

            foreach (
                string warning in warnings.Where(warning =>
                    !allowedWarnings.Contains(value: warning, comparer: StringComparer.OrdinalIgnoreCase)
                )
            )
            {
                this._logger.GlobalConfigurationHidesWarning(projectName: projectName, warning: warning);
            }
        }
    }

    private static IReadOnlyList<string> ExtractWarnings(string value)
    {
        return
        [
            .. value
                .Split(separator: ';')
                .Where(predicate: HasContent)
                .SelectMany(selector: static s => s.Split(separator: ',').Where(predicate: HasContent))
                .Where(predicate: static s => !string.IsNullOrWhiteSpace(s))
                .Order(StringComparer.OrdinalIgnoreCase),
        ];
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
