using System.Diagnostics.CodeAnalysis;

namespace FunFair.BuildCheck.SolutionChecks.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
    internal sealed class GlobalJsonPacket
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public GlobalJsonSdk? Sdk { get; set; }
    }
}