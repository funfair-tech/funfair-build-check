using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Helpers;

internal static class ProjectValueHelpers
{
    private static readonly IReadOnlyList<string> PackagesForTestProjectDetection = new[]
                                                                                    {
                                                                                        "xunit",
                                                                                        "xunit.runner.visualstudio",
                                                                                        "NSubstitute",
                                                                                        "Microsoft.NET.Test.Sdk",
                                                                                        "TeamCity.VSTest.TestAdapter",
                                                                                        "FunFair.Test.Common"
                                                                                    };

    public static bool IsDotNetTool(this XmlDocument project)
    {
        XmlNode? outputTypeNode = project.SelectSingleNode("/Project/PropertyGroup/PackAsTool");

        if (outputTypeNode != null)
        {
            return !string.IsNullOrWhiteSpace(outputTypeNode.InnerText) && StringComparer.InvariantCultureIgnoreCase.Equals(x: outputTypeNode.InnerText, y: "True");
        }

        return false;
    }

    public static string GetSdk(this XmlDocument project)
    {
        XmlElement? projectElement = project.SelectSingleNode("/Project") as XmlElement;

        if (projectElement != null)
        {
            return projectElement.GetAttribute("Sdk");
        }

        return string.Empty;
    }

    public static bool IsTestProject(this XmlDocument project, string projectName, ILogger logger)
    {
        XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

        if (nodes == null)
        {
            return false;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
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
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);

        if (nodes != null)
        {
            foreach (XmlElement item in nodes.OfType<XmlElement>())
            {
                XmlElement? propertyGroup = item.ParentNode as XmlElement;

                if (propertyGroup == null)
                {
                    continue;
                }

                string condition = propertyGroup.GetAttribute(name: "Condition");

                if (string.IsNullOrWhiteSpace(condition))
                {
                    hasGlobalSetting = true;
                }
            }
        }

        XmlNodeList? configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

        if (configurationGroups != null)
        {
            foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
            {
                XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

                if (node != null)
                {
                    continue;
                }

                if (hasGlobalSetting)
                {
                    continue;
                }

                string configuration = propertyGroup.GetAttribute(name: "Condition");
                logger.LogError($"{projectName}: Configuration {configuration} should specify {nodePresence}");
            }
        }

        if (!hasGlobalSetting && configurationGroups?.Count == 0)
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
        CheckValueCommon(projectName: projectName, project: project, nodePresence: nodePresence, isRequiredValue: isRequiredValue, requiredValueDisplayText: msg, logger: logger);
    }

    private static void CheckValueCommon(string projectName, XmlDocument project, string nodePresence, Func<string, bool> isRequiredValue, string requiredValueDisplayText, ILogger logger)
    {
        bool hasGlobalSetting = CheckGlobalSettings(project: project, nodePresence: nodePresence, isRequiredValue: isRequiredValue);

        XmlNodeList? configurationGroups = project.SelectNodes(xpath: "/Project/PropertyGroup[@Condition]");

        if (configurationGroups != null)
        {
            CheckConditionalSettings(projectName: projectName,
                                     nodePresence: nodePresence,
                                     isRequiredValue: isRequiredValue,
                                     requiredValueDisplayText: requiredValueDisplayText,
                                     configurationGroups: configurationGroups,
                                     hasGlobalSetting: hasGlobalSetting,
                                     logger: logger);
        }

        if (!hasGlobalSetting && configurationGroups?.Count == 0)
        {
            logger.LogError($"{projectName}: Should specify {nodePresence} as {requiredValueDisplayText}.");
        }
    }

    private static void CheckConditionalSettings(string projectName,
                                                 string nodePresence,
                                                 Func<string, bool> isRequiredValue,
                                                 string requiredValueDisplayText,
                                                 XmlNodeList configurationGroups,
                                                 bool hasGlobalSetting,
                                                 ILogger logger)
    {
        foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
        {
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
    }

    private static bool CheckGlobalSettings(XmlDocument project, string nodePresence, Func<string, bool> isRequiredValue)
    {
        bool hasGlobalSetting = false;
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup[not(@Condition)]/" + nodePresence);

        if (nodes != null)
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

                if (isRequiredValue(value))
                {
                    hasGlobalSetting = true;
                }
            }
        }

        return hasGlobalSetting;
    }

    private static string GetTextValue(XmlNode node)
    {
        return node.InnerText;
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

        return GetStringProperty(project: project, path: "/Project/PropertyGroup/OutputType", defaultType: defaultType);
    }

    private static string GetStringProperty(XmlDocument project, string path, string defaultType)
    {
        XmlNode? outputTypeNode = project.SelectSingleNode(path);

        if (outputTypeNode != null)
        {
            return string.IsNullOrWhiteSpace(outputTypeNode.InnerText)
                ? defaultType
                : outputTypeNode.InnerText;
        }

        return defaultType;
    }

    public static string GetRuntimeIdentifiers(this XmlDocument project)
    {
        const string defaultType = @"";

        return GetStringProperty(project: project, path: "/Project/PropertyGroup/RuntimeIdentifiers", defaultType: defaultType);
    }

    public static bool IsPackable(this XmlDocument project)
    {
        string value = GetStringProperty(project: project, path: "/Project/PropertyGroup/IsPackable", defaultType: "true");

        return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "true");
    }

    public static bool IsPublishable(this XmlDocument project)
    {
        string value = GetStringProperty(project: project, path: "/Project/PropertyGroup/IsPublishable", defaultType: "true");

        return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "true");
    }

    public static string? GetAwsProjectType(this XmlDocument project)
    {
        XmlNode? outputTypeNode = project.SelectSingleNode("/Project/PropertyGroup/AWSProjectType");

        return outputTypeNode?.InnerText;
    }

    public static bool HasProperty(this XmlDocument project, string property)
    {
        XmlNode? outputTypeNode = project.SelectSingleNode("/Project/PropertyGroup/" + property);

        if (outputTypeNode != null)
        {
            return !string.IsNullOrWhiteSpace(outputTypeNode.InnerText);
        }

        return false;
    }
}