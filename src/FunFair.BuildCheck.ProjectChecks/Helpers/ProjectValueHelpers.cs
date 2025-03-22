using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers.LoggingExtensions;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Helpers;

internal static class ProjectValueHelpers
{
    private static readonly IReadOnlyList<string> PackagesForTestProjectDetection =
    [
        "xunit",
        "xunit.v3",
        "NSubstitute",
        "FunFair.Test.Common",
    ];

    public static bool IsDotNetTool(in this ProjectContext project)
    {
        XmlNode? outputTypeNode = project.CsProjXml.SelectSingleNode(
            "/Project/PropertyGroup/PackAsTool"
        );

        if (outputTypeNode is not null)
        {
            return !string.IsNullOrWhiteSpace(outputTypeNode.InnerText)
                && StringComparer.InvariantCultureIgnoreCase.Equals(
                    x: outputTypeNode.InnerText,
                    y: "True"
                );
        }

        return false;
    }

    public static string GetSdk(in this ProjectContext project)
    {
        XmlElement? projectElement = project.CsProjXml.SelectSingleNode("/Project") as XmlElement;

        if (projectElement is not null)
        {
            return projectElement.GetAttribute("Sdk");
        }

        return string.Empty;
    }

    public static bool ReferencesPackages(
        in this ProjectContext project,
        string packageName,
        ILogger logger
    )
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/PackageReference"
        );

        if (nodes is null)
        {
            return false;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string includedPackageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(includedPackageName))
            {
                logger.ContainsBadReferenceToPackages(project.Name);

                continue;
            }

            if (
                StringComparer.InvariantCultureIgnoreCase.Equals(
                    x: packageName,
                    y: includedPackageName
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsTestProject(in this ProjectContext project, ILogger logger)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/PackageReference"
        );

        if (nodes is null)
        {
            return false;
        }

        foreach (XmlElement reference in nodes.OfType<XmlElement>())
        {
            string packageName = reference.GetAttribute(name: "Include");

            if (string.IsNullOrWhiteSpace(packageName))
            {
                logger.ContainsBadReferenceToPackages(project.Name);

                continue;
            }

            if (
                PackagesForTestProjectDetection.Any(x =>
                    StringComparer.InvariantCultureIgnoreCase.Equals(x: x, y: packageName)
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    public static void CheckNode(in ProjectContext project, string nodePresence, ILogger logger)
    {
        bool hasGlobalSetting = false;
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            "/Project/PropertyGroup[not(@Condition)]/" + nodePresence
        );

        if (nodes is not null)
        {
            foreach (XmlElement item in nodes.OfType<XmlElement>())
            {
                if (item.ParentNode is not XmlElement propertyGroup)
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

        XmlNodeList? configurationGroups = project.CsProjXml.SelectNodes(
            xpath: "/Project/PropertyGroup[@Condition]"
        );

        if (configurationGroups is not null)
        {
            foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
            {
                XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

                if (node is not null)
                {
                    continue;
                }

                if (hasGlobalSetting)
                {
                    continue;
                }

                string configuration = propertyGroup.GetAttribute(name: "Condition");
                logger.ConfigurationShouldSpecifyNodePrescence(
                    projectName: project.Name,
                    configuration: configuration,
                    nodePresence: nodePresence
                );
            }
        }

        if (!hasGlobalSetting && configurationGroups?.Count == 0)
        {
            logger.ProjectShouldSpecifyNodePrescence(
                projectName: project.Name,
                nodePresence: nodePresence
            );
        }
    }

    public static void CheckValue(
        in ProjectContext project,
        string nodePresence,
        bool requiredValue,
        ILogger logger
    )
    {
        CheckValueCommon(
            project: project,
            nodePresence: nodePresence,
            isRequiredValue: v => IsRequiredValue(requiredValue: requiredValue, value: v),
            requiredValue.ToString(CultureInfo.InvariantCulture),
            logger: logger
        );
    }

    public static void CheckValue(
        in ProjectContext project,
        string nodePresence,
        string requiredValue,
        ILogger logger
    )
    {
        CheckValueCommon(
            project: project,
            nodePresence: nodePresence,
            isRequiredValue: v => IsRequiredValue(requiredValue: requiredValue, value: v),
            requiredValueDisplayText: requiredValue,
            logger: logger
        );
    }

    public static void CheckValue(
        in ProjectContext project,
        string nodePresence,
        Func<string, bool> isRequiredValue,
        string msg,
        ILogger logger
    )
    {
        CheckValueCommon(
            project: project,
            nodePresence: nodePresence,
            isRequiredValue: isRequiredValue,
            requiredValueDisplayText: msg,
            logger: logger
        );
    }

    private static void CheckValueCommon(
        in ProjectContext project,
        string nodePresence,
        Func<string, bool> isRequiredValue,
        string requiredValueDisplayText,
        ILogger logger
    )
    {
        bool hasGlobalSetting = CheckGlobalSettings(
            project: project,
            nodePresence: nodePresence,
            isRequiredValue: isRequiredValue
        );

        XmlNodeList? configurationGroups = project.CsProjXml.SelectNodes(
            xpath: "/Project/PropertyGroup[@Condition]"
        );

        if (configurationGroups is not null)
        {
            CheckConditionalSettings(
                projectName: project.Name,
                nodePresence: nodePresence,
                isRequiredValue: isRequiredValue,
                requiredValueDisplayText: requiredValueDisplayText,
                configurationGroups: configurationGroups,
                hasGlobalSetting: hasGlobalSetting,
                logger: logger
            );
        }

        if (!hasGlobalSetting && configurationGroups?.Count == 0)
        {
            logger.ProjectShouldSpecifyNodePrescenceAsValue(
                projectName: project.Name,
                nodePresence: nodePresence,
                requiredValueDisplayText: requiredValueDisplayText
            );
        }
    }

    private static void CheckConditionalSettings(
        string projectName,
        string nodePresence,
        Func<string, bool> isRequiredValue,
        string requiredValueDisplayText,
        XmlNodeList configurationGroups,
        bool hasGlobalSetting,
        ILogger logger
    )
    {
        foreach (XmlElement propertyGroup in configurationGroups.OfType<XmlElement>())
        {
            XmlNode? node = propertyGroup.SelectSingleNode(nodePresence);

            Check(node: node, propertyGroup: propertyGroup);
        }

        void Check(XmlNode? node, XmlElement propertyGroup)
        {
            if (node is null)
            {
                if (hasGlobalSetting)
                {
                    return;
                }

                logger.ConfigurationShouldSpecifyNodePrescence(
                    projectName: projectName,
                    propertyGroup.GetAttribute(name: "Condition"),
                    nodePresence: nodePresence
                );

                return;
            }

            string value = GetTextValue(node);

            if (isRequiredValue(value) || hasGlobalSetting)
            {
                return;
            }

            logger.ConfigurationShouldSpecifyNodePrescenceAsValue(
                projectName: projectName,
                propertyGroup.GetAttribute(name: "Condition"),
                nodePresence: nodePresence,
                requiredValueDisplayText: requiredValueDisplayText
            );
        }
    }

    private static bool CheckGlobalSettings(
        in ProjectContext project,
        string nodePresence,
        Func<string, bool> isRequiredValue
    )
    {
        bool hasGlobalSetting = false;
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            "/Project/PropertyGroup[not(@Condition)]/" + nodePresence
        );

        if (nodes is not null)
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
        return !string.IsNullOrWhiteSpace(value)
            && StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: requiredValue);
    }

    public static string GetOutputType(in this ProjectContext project)
    {
        const string defaultType = "Library";

        return GetStringProperty(
            project: project,
            path: "/Project/PropertyGroup/OutputType",
            defaultType: defaultType
        );
    }

    private static string GetStringProperty(
        in ProjectContext project,
        string path,
        string defaultType
    )
    {
        XmlNode? outputTypeNode = project.CsProjXml.SelectSingleNode(path);

        if (outputTypeNode is not null)
        {
            return string.IsNullOrWhiteSpace(outputTypeNode.InnerText)
                ? defaultType
                : outputTypeNode.InnerText;
        }

        return defaultType;
    }

    public static string GetRuntimeIdentifiers(in this ProjectContext project)
    {
        const string defaultType = "";

        return GetStringProperty(
            project: project,
            path: "/Project/PropertyGroup/RuntimeIdentifiers",
            defaultType: defaultType
        );
    }

    public static bool IsPackable(in this ProjectContext project)
    {
        string value = GetStringProperty(
            project: project,
            path: "/Project/PropertyGroup/IsPackable",
            defaultType: "true"
        );

        return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "true");
    }

    public static bool IsPublishable(in this ProjectContext project)
    {
        string value = GetStringProperty(
            project: project,
            path: "/Project/PropertyGroup/IsPublishable",
            defaultType: "true"
        );

        return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "true");
    }

    public static bool IsAnalyzerOrSourceGenerator(in this ProjectContext project)
    {
        return project.IsAnalyzer() || project.IsSourceGenerator();
    }

    private static bool IsAnalyzer(in this ProjectContext project)
    {
        return project.HasProjectImport("$(SolutionDir)Analyzer.props");
    }

    private static bool IsSourceGenerator(in this ProjectContext project)
    {
        return project.HasProjectImport("$(SolutionDir)SourceGenerator.props");
    }

    public static bool HasProjectImport(in this ProjectContext project, string projectImport)
    {
        bool found = false;
        XmlNodeList? imports = project.CsProjXml.SelectNodes("/Project/Import[@Project]");

        if (imports is not null)
        {
            found = imports
                .OfType<XmlElement>()
                .Select(import => import.GetAttribute(name: "Project"))
                .Any(candidate =>
                    StringComparer.InvariantCultureIgnoreCase.Equals(x: candidate, y: projectImport)
                );
        }

        return found;
    }

    public static string? GetAwsProjectType(in this ProjectContext project)
    {
        return project.GetProperty("AWSProjectType");
    }

    public static string? GetProperty(in this ProjectContext project, string property)
    {
        XmlNode? outputTypeNode = project.CsProjXml.SelectSingleNode(
            "/Project/PropertyGroup/" + property
        );

        if (outputTypeNode is not null)
        {
            if (!string.IsNullOrWhiteSpace(outputTypeNode.InnerText))
            {
                return outputTypeNode.InnerText;
            }
        }

        return null;
    }

    public static bool HasProperty(in this ProjectContext project, string property)
    {
        string? value = project.GetProperty(property);

        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool IsFunFairTestProject(in this ProjectContext project)
    {
        XmlNode? outputTypeNode = project.CsProjXml.SelectSingleNode(
            "/Project/PropertyGroup/FFTestProject"
        );

        if (outputTypeNode is not null)
        {
            return !string.IsNullOrWhiteSpace(outputTypeNode.InnerText)
                && StringComparer.InvariantCultureIgnoreCase.Equals(
                    x: outputTypeNode.InnerText,
                    y: "true"
                );
        }

        return false;
    }
}
