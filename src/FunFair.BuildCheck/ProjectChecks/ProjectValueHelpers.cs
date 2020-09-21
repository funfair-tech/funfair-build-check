using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public static class ProjectValueHelpers
    {
        private static readonly IReadOnlyList<string> PackagesForTestProjectDetection = new[] {"Xunit", "NSubstitute", "Microsoft.NET.Test.Sdk", "FunFair.Test.Common"};

        public static bool IsTestProject(this XmlDocument project, string projectName, ILogger logger)
        {
            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            foreach (XmlElement? reference in nodes)
            {
                if (reference == null)
                {
                    continue;
                }

                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                if (PackagesForTestProjectDetection.Any(x => StringComparer.InvariantCultureIgnoreCase.Equals(x: x, y: packageName)))
                {
                    return true;
                }
            }

            return false;
        }

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
            CheckValueCommon(projectName: projectName,
                             project: project,
                             nodePresence: nodePresence,
                             isRequiredValue: v => IsRequiredValue(requiredValue: requiredValue, value: v),
                             requiredValue.ToString(CultureInfo.InvariantCulture),
                             logger: logger);
        }

        public static void CheckValue(string projectName, XmlDocument project, string nodePresence, string requiredValue, ILogger logger)
        {
            CheckValueCommon(projectName: projectName,
                             project: project,
                             nodePresence: nodePresence,
                             isRequiredValue: v => IsRequiredValue(requiredValue: requiredValue, value: v),
                             requiredValueDisplayText: requiredValue,
                             logger: logger);
        }

        public static void CheckValue(string projectName, XmlDocument project, string nodePresence, Func<string, bool> isRequiredValue, string msg, ILogger logger)
        {
            CheckValueCommon(projectName: projectName,
                             project: project,
                             nodePresence: nodePresence,
                             isRequiredValue: isRequiredValue,
                             requiredValueDisplayText: msg,
                             logger: logger);
        }

        private static void CheckValueCommon(string projectName,
                                             XmlDocument project,
                                             string nodePresence,
                                             Func<string, bool> isRequiredValue,
                                             string requiredValueDisplayText,
                                             ILogger logger)
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
            return IsRequiredValue(requiredValue.ToString(CultureInfo.InvariantCulture), value: value);
        }

        private static bool IsRequiredValue(string requiredValue, string value)
        {
            return !string.IsNullOrWhiteSpace(value) && StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: requiredValue);
        }

        public static string GetOutputType(this XmlDocument project)
        {
            const string defaultType = @"Library";

            XmlNode? outputTypeNode = project.SelectSingleNode("/Project/PropertyGroup/OutputType");

            if (outputTypeNode != null)
            {
                return outputTypeNode.InnerText ?? defaultType;
            }

            return defaultType;
        }

        public static string? GetAwsProjectType(this XmlDocument project)
        {
            XmlNode? outputTypeNode = project.SelectSingleNode("/Project/PropertyGroup/AWSProjectType");

            if (outputTypeNode != null)
            {
                return outputTypeNode.InnerText;
            }

            return null;
        }
    }
}