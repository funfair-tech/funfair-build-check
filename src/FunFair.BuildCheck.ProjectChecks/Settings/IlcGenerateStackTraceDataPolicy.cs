using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IlcGenerateStackTraceDataPolicy : IProjectCheck
{
    private const string EXPECTED = @"false";

    private readonly ILogger<IlcGenerateStackTraceDataPolicy> _logger;

    public IlcGenerateStackTraceDataPolicy(ILogger<IlcGenerateStackTraceDataPolicy> logger)
    {
        this._logger = logger;
    }

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IlcGenerateStackTraceData", requiredValue: EXPECTED, logger: this._logger);
    }
}