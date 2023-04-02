using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustSpecifyOutputType : IProjectCheck
{
    private readonly ILogger<MustSpecifyOutputType> _logger;

    public MustSpecifyOutputType(ILogger<MustSpecifyOutputType> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        const string msg = "Exe or Library";

        static bool IsRequiredValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Exe") || StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Library");
        }

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"OutputType", isRequiredValue: IsRequiredValue, msg: msg, logger: this._logger);
    }
}