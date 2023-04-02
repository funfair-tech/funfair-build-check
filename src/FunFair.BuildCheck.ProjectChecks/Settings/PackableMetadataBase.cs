using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public abstract class PackableMetadataBase : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _property;

    protected PackableMetadataBase(string property, ILogger logger)
    {
        this._property = property ?? throw new ArgumentNullException(nameof(property));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        if (!project.IsPackable())
        {
            return;
        }

        if (!project.HasProperty(this._property))
        {
            this._logger.LogError($"Packable project {projectName} does not define {this._property}");
        }
    }
}