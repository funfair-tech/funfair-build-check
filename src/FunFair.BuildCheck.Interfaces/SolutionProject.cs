using System.Diagnostics;

namespace FunFair.BuildCheck.Interfaces;

[DebuggerDisplay("{DisplayName}: {FileName}")]
public sealed class SolutionProject
{
    public SolutionProject(string fileName, string displayName)
    {
        this.FileName = fileName;
        this.DisplayName = displayName;
    }

    public string FileName { get; }

    public string DisplayName { get; }
}
