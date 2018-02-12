using System;
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
            foreach (XmlElement item in nodes)
            {
                XmlElement propertyGroup = (XmlElement) item.ParentNode;
                string condition = propertyGroup.GetAttribute("Condition");
                if (string.IsNullOrWhiteSpace(condition)) hasGlobalSetting = true;
            }

            XmlNodeList configurationGroups = project.SelectNodes("/Project/PropertyGroup[@Condition]");
            foreach (XmlElement propertyGroup in configurationGroups)
            {
                XmlNode node = propertyGroup.SelectSingleNode(nodePresence);
                if (node == null)
                    if (!hasGlobalSetting)
                    {
                        string configuration = propertyGroup.GetAttribute("Condition");
                        logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence}");
                    }
            }

            if (!hasGlobalSetting && configurationGroups.Count == 0) logger.LogError($"{projectName}: Should specify {nodePresence}.");
        }

        public static void CheckValue(string projectName, XmlDocument project, string nodePresence, bool requiredValue, ILogger logger)
        {
            bool hasGlobalSetting = false;
            XmlNodeList nodes = project.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);
            foreach (XmlElement item in nodes)
            {
                XmlElement propertyGroup = (XmlElement) item.ParentNode;
                string condition = propertyGroup.GetAttribute("Condition");
                if (string.IsNullOrWhiteSpace(condition))
                {
                    string value = GetTextValue(item);
                    if (IsRequiredValue(requiredValue, value)) hasGlobalSetting = true;
                }
            }

            XmlNodeList configurationGroups = project.SelectNodes("/Project/PropertyGroup[@Condition]");
            foreach (XmlElement propertyGroup in configurationGroups)
            {
                XmlNode node = propertyGroup.SelectSingleNode(nodePresence);
                if (node == null)
                {
                    if (!hasGlobalSetting)
                    {
                        string configuration = propertyGroup.GetAttribute("Condition");
                        logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence}.");
                    }
                }
                else
                {
                    string value = GetTextValue(node);
                    if (!IsRequiredValue(requiredValue, value))
                        if (!hasGlobalSetting)
                        {
                            string configuration = propertyGroup.GetAttribute("Condition");
                            logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence} as {requiredValue}.");
                        }
                }
            }

            if (!hasGlobalSetting && configurationGroups.Count == 0) logger.LogError($"{projectName}: Should specify {nodePresence} as {requiredValue}.");
        }

        private static string GetTextValue(XmlNode node)
        {
            return node.InnerText ?? string.Empty;
        }

        private static bool IsRequiredValue(bool requiredValue, string value)
        {
            return !string.IsNullOrWhiteSpace(value) && StringComparer.InvariantCultureIgnoreCase.Equals(value, requiredValue.ToString());
        }
    }
}