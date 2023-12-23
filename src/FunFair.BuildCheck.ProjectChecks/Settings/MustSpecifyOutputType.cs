using System;
using System.Threading;
using System.Threading.Tasks;
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
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        const string msg = "Exe or Library";

        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: "OutputType", isRequiredValue: IsRequiredValue, msg: msg, logger: this._logger);

        return ValueTask.CompletedTask;
    }

    private static bool IsRequiredValue(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && IsExeOrLibrary(value);
    }

    private static bool IsExeOrLibrary(string value)
    {
        return StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Exe") || StringComparer.InvariantCultureIgnoreCase.Equals(x: value, y: "Library");
    }
}