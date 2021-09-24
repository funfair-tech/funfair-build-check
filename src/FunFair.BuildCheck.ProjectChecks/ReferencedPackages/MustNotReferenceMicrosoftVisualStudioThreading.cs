using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that Microsoft.VisualStudio.Threading package is not referenced.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustNotReferenceMicrosoftVisualStudioThreading : MustNotReferencePackages
    {
        public MustNotReferenceMicrosoftVisualStudioThreading(ILogger<MustNotReferenceMicrosoftVisualStudioThreading> logger)
            : base(new[] { @"Microsoft.VisualStudio.Threading" }, reason: "Obsoleted with .net core 3.1", logger: logger)
        {
        }
    }
}