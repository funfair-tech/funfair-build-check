using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotReferenceMicrosoftVisualStudioThreading : MustNotReferencePackages
{
    private static readonly string[] PackageIds = ["Microsoft.VisualStudio.Threading"];

    public MustNotReferenceMicrosoftVisualStudioThreading(ILogger<MustNotReferenceMicrosoftVisualStudioThreading> logger)
        : base(packageIds: PackageIds, reason: "Obsoleted with .net core 3.1", logger: logger) { }
}
