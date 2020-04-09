using System.Diagnostics.CodeAnalysis;

namespace BuildCheck.SolutionChecks.Models
{
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
    internal sealed class GlobalJsonSdk
    {
        public string? Version { get; set; }

        public string? RollForward { get; set; }

        public bool? AllowPrerelease { get; set; }
    }
}