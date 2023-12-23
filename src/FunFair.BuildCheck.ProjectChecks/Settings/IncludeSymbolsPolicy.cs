using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class IncludeSymbolsPolicy : SimplePropertyProjectCheckBase
{
    public IncludeSymbolsPolicy(ILogger<IncludeSymbolsPolicy> logger)
        : base(propertyName: "IncludeSymbols", requiredValue: "true", logger: logger)
    {
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPackable();
    }
}