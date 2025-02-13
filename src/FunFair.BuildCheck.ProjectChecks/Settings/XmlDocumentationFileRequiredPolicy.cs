using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class XmlDocumentationFileRequiredPolicy : IProjectCheck
{
    private const string EXPECTED =
        @"bin\$(Configuration)\$(TargetFramework)\$(MSBuildProjectName).xml";
    private readonly ILogger<XmlDocumentationFileRequiredPolicy> _logger;

    private readonly IRepositorySettings _repositorySettings;

    public XmlDocumentationFileRequiredPolicy(
        IRepositorySettings repositorySettings,
        ILogger<XmlDocumentationFileRequiredPolicy> logger
    )
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
    }

    public ValueTask CheckAsync(
        string projectName,
        string projectFolder,
        XmlDocument project,
        CancellationToken cancellationToken
    )
    {
        if (!this._repositorySettings.XmlDocumentationRequired)
        {
            return ValueTask.CompletedTask;
        }

        bool testProject = project.IsTestProject(projectName: projectName, logger: this._logger);

        if (testProject && this._repositorySettings.IsUnitTestBase)
        {
            testProject = projectName.EndsWith(
                value: ".Tests",
                comparisonType: StringComparison.OrdinalIgnoreCase
            );
        }

        if (testProject)
        {
            this.CheckTestProject(projectName: projectName, project: project);

            return ValueTask.CompletedTask;
        }

        ProjectValueHelpers.CheckValue(
            projectName: projectName,
            project: project,
            nodePresence: "DocumentationFile",
            requiredValue: EXPECTED,
            logger: this._logger
        );

        return ValueTask.CompletedTask;
    }

    private void CheckTestProject(string projectName, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

        if (nodes is not null && nodes.Count != 0)
        {
            this._logger.TestProjectsShouldNotHaveXmlDocumentation(projectName);
        }
    }
}
