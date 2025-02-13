using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

internal static class GlobalJsonHelpers
{
    public static bool GetFileNameForSolution(
        string solutionFileName,
        [NotNullWhen(true)] out string? file
    )
    {
        string? solutionDir = Path.GetDirectoryName(solutionFileName);

        if (solutionDir is null)
        {
            file = null;
            return false;
        }

        string candidate = Path.Combine(path1: solutionDir, path2: "global.json");

        if (!File.Exists(candidate))
        {
            file = null;

            return false;
        }

        file = candidate;
        return true;
    }
}
