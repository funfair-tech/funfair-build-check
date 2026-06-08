namespace FunFair.BuildCheck.SolutionChecks.Helpers;

public sealed class GlobalJsonNotFoundResult : GlobalJsonLoadResult
{
    public static GlobalJsonNotFoundResult Instance { get; } = new();
}
