using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MustNotDisableUnexpectedWarnings : IProjectCheck
    {
        private static readonly IReadOnlyList<string> AllowedWarnings = new[]
                                                                        {
                                                                            // Xml Docs
                                                                            "1591"
                                                                        };

        private static readonly IReadOnlyList<string> AllowedTestProjectWarnings = Array.Empty<string>();

        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustNotDisableUnexpectedWarnings(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            bool isTestProject = project.IsTestProject(projectName: projectName, logger: this._logger);

            IReadOnlyList<string> allowedWarnings = isTestProject ? AllowedTestProjectWarnings : AllowedWarnings;

            const string nodePresence = @"NoWarn";
            XmlNodeList nodes = project.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);

            foreach (XmlElement? item in nodes)
            {
                if (item == null)
                {
                    continue;
                }

                XmlElement propertyGroup = (XmlElement) item.ParentNode;
                string condition = propertyGroup.GetAttribute(name: "Condition");

                if (string.IsNullOrWhiteSpace(condition))
                {
                    string value = GetTextValue(item);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        string[] warnings = ExtractWarnings(value);

                        foreach (string warning in warnings)
                        {
                            if (!allowedWarnings.Contains(warning))
                            {
                                this._logger.LogError($"{projectName}: Global Configuration hides warning {warning}.");
                            }
                        }
                    }
                }
            }

            XmlNodeList configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

            foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
            {
                XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

                if (node != null)
                {
                    string value = GetTextValue(node);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        string configuration = propertyGroup.GetAttribute(name: "Condition");

                        string[] warnings = ExtractWarnings(value);

                        foreach (string warning in warnings)
                        {
                            if (warning == "$(NoWarn)")
                            {
                                // skip references to global configs
                                continue;
                            }

                            if (!allowedWarnings.Contains(warning))
                            {
                                this._logger.LogError($"{projectName}: Configuration {configuration} hides warning {warning}.");
                            }
                        }
                    }
                }
            }
        }

        private static string[] ExtractWarnings(string value)
        {
            return value.Split(separator: ";")
                        .Where(predicate: s => !string.IsNullOrWhiteSpace(s))
                        .SelectMany(selector: s => s.Split(separator: ",")
                                                    .Where(predicate: t => !string.IsNullOrWhiteSpace(t)))
                        .Where(predicate: s => !string.IsNullOrWhiteSpace(s))
                        .OrderBy(keySelector: s => s.ToLowerInvariant())
                        .ToArray();
        }

        private static string GetTextValue(XmlNode node)
        {
            return node.InnerText ?? string.Empty;
        }
    }
}