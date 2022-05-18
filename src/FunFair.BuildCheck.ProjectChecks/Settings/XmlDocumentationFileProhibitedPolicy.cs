using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the DocumentationFile is set appropriately
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class XmlDocumentationFileProhibitedPolicy : IProjectCheck
{
    private readonly ILogger<XmlDocumentationFileProhibitedPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public XmlDocumentationFileProhibitedPolicy(ILogger<XmlDocumentationFileProhibitedPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        XmlNodeList? nodes = project.SelectNodes("/Project/PropertyGroup/DocumentationFile");

        if (nodes != null && nodes.Count != 0)
        {
            this._logger.LogError($"{projectName}: Should not have XML Documentation");
        }
    }
}