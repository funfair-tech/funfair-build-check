using System.Diagnostics.CodeAnalysis;

namespace FunFair.BuildCheck.SolutionChecks.Models
{
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
    internal sealed class GlobalJsonPacket
    {
        [SuppressMessage(category: "ReSharper", checkId: "UnusedAutoPropertyAccessor.Global", Justification = "TODO: Review")]
        public GlobalJsonSdk? Sdk { get; set; }
    }
}