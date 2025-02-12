using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustHaveCsharpierPackage : MustHaveAnalyzerPackage
{
    public MustHaveCsharpierPackage(ILogger<MustHaveCsharpierPackage> logger)
        : base(packageId: "CSharpier.MSBuild", mustHave: true, logger: logger) { }
}