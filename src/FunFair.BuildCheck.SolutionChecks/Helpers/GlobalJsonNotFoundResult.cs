using System.Diagnostics;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

[DebuggerDisplay("Not found")]
public sealed class GlobalJsonNotFoundResult : GlobalJsonLoadResult
{
    public static GlobalJsonNotFoundResult Instance { get; } = new();
}
