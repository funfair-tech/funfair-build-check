using System.Diagnostics.CodeAnalysis;

namespace FunFair.BuildCheck.SolutionChecks.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
    internal sealed class GlobalJsonSdk
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? Version { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? RollForward { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool? AllowPrerelease { get; set; }
    }
}