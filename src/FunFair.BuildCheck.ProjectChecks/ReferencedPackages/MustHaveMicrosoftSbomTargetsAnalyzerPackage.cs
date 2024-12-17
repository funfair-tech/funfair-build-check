using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveMicrosoftSbomTargetsAnalyzerPackage : MustHaveAnalyzerPackage
{
    public MustHaveMicrosoftSbomTargetsAnalyzerPackage(ILogger<MustHaveMicrosoftSbomTargetsAnalyzerPackage> logger)
        : base(packageId: "Microsoft.Sbom.Targets", mustHave: true, logger: logger)
    {
    }

    protected override bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return project.IsPackable();
    }
}