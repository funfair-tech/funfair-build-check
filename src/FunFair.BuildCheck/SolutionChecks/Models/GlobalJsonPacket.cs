using System.Diagnostics.CodeAnalysis;

namespace FunFair.BuildCheck.SolutionChecks.Models
{
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
    internal sealed class GlobalJsonPacket
    {
        public GlobalJsonSdk? Sdk { get; set; }
    }
}