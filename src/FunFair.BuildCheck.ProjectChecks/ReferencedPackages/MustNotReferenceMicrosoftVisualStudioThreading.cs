using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class MustNotReferenceMicrosoftVisualStudioThreading : MustNotReferencePackages
{
    public MustNotReferenceMicrosoftVisualStudioThreading(ILogger<MustNotReferenceMicrosoftVisualStudioThreading> logger)
        : base(new[]
               {
                   @"Microsoft.VisualStudio.Threading"
               },
               reason: "Obsoleted with .net core 3.1",
               logger: logger)
    {
    }
}