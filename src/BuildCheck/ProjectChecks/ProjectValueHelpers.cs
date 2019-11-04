using System;
using System.Globalization;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public static class ProjectValueHelpers
    {
        public static void CheckNode(string projectName, XmlDocument project, string nodePresence, ILogger logger)
        {
            bool hasGlobalSetting = false;
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
                    hasGlobalSetting = true;
                }
            }

            XmlNodeList configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

            foreach (XmlElement? propertyGroup in configurationGroups)
            {
                if (propertyGroup == null)
                {
                    continue;
                }

                XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

                if (node == null)
                {
                    if (!hasGlobalSetting)
                    {
                        string configuration = propertyGroup.GetAttribute(name: "Condition");
                        logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence}");
                    }
                }
            }

            if (!hasGlobalSetting && configurationGroups.Count == 0)
            {
                logger.LogError($"{projectName}: Should specify {nodePresence}.");
            }
        }

        public static void CheckValue(string projectName, XmlDocument project, string nodePresence, bool requiredValue, ILogger logger)
        {
            CheckValueCommon(projectName, project, nodePresence, isRequiredValue: v => IsRequiredValue(requiredValue, v), requiredValue.ToString(CultureInfo.InvariantCulture), logger);
        }

        public static void CheckValue(string projectName, XmlDocument project, string nodePresence, string requiredValue, ILogger logger)
        {
            CheckValueCommon(projectName, project, nodePresence, isRequiredValue: v => IsRequiredValue(requiredValue, v), requiredValue, logger);
        }

        private static void CheckValueCommon(string projectName, XmlDocument project, string nodePresence, Func<string, bool> isRequiredValue, string requiredValueDisplayText, ILogger logger)
        {
            bool hasGlobalSetting = false;
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

                    if (isRequiredValue(value))
                    {
                        hasGlobalSetting = true;
                    }
                }
            }

            XmlNodeList configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

            foreach (XmlElement? propertyGroup in configurationGroups)
            {
                if (propertyGroup == null)
                {
                    continue;
                }

                XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

                if (node == null)
                {
                    if (!hasGlobalSetting)
                    {
                        string configuration = propertyGroup.GetAttribute(name: "Condition");
                        logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence}.");
                    }
                }
                else
                {
                    string value = GetTextValue(node);

                    if (!isRequiredValue(value))
                    {
                        if (!hasGlobalSetting)
                        {
                            string configuration = propertyGroup.GetAttribute(name: "Condition");
                            logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence} as {requiredValueDisplayText}.");
                        }
                    }
                }
            }

            if (!hasGlobalSetting && configurationGroups.Count == 0)
            {
                logger.LogError($"{projectName}: Should specify {nodePresence} as {requiredValueDisplayText}.");
            }
        }

        private static string GetTextValue(XmlNode node)
        {
            return node.InnerText ?? string.Empty;
        }

        private static bool IsRequiredValue(bool requiredValue, string value)
        {
            return IsRequiredValue(requiredValue.ToString(CultureInfo.InvariantCulture), value);
        }

        private static bool IsRequiredValue(string requiredValue, string value)
        {
            return !string.IsNullOrWhiteSpace(value) && StringComparer.InvariantCultureIgnoreCase.Equals(value, requiredValue);
        }
    }
}