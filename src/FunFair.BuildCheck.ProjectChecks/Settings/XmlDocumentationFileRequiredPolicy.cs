using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XmlDocumentationFileRequiredPolicy : IProjectCheck
{
    private const string EXPECTED = @"bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml";
    private readonly ILogger<XmlDocumentationFileRequiredPolicy> _logger;

    private readonly IRepositorySettings _repositorySettings;

    public XmlDocumentationFileRequiredPolicy(IRepositorySettings repositorySettings, ILogger<XmlDocumentationFileRequiredPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!this._repositorySettings.XmlDocumentationRequired)
        {
            return;
        }

        bool testProject = project.IsTestProject(projectName: projectName, logger: this._logger);

        if (testProject && this._repositorySettings.IsUnitTestBase)
        {
            testProject = projectName.EndsWith(value: ".Tests", comparisonType: StringComparison.OrdinalIgnoreCase);
        }

        if (testProject)
        {
            this.CheckTestProject(projectName: projectName, project: project);

            return;
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"DocumentationFile", requiredValue: EXPECTED, logger: this._logger);
    }

    private void CheckTestProject(string projectName, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

        if (nodes != null && nodes.Count != 0)
        {
            this._logger.LogError($"{projectName}: Test projects should not have XML Documentation");
        }
    }
}