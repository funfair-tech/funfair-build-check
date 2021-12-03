using System.Diagnostics;

namespace FunFair.BuildCheck.Interfaces;

/// <summary>
///     A Project
/// </summary>
[DebuggerDisplay("{DisplayName}: {FileName}")]
public sealed class Project
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="fileName">The full path to the project.</param>
    /// <param name="displayName">The display name of the project as in the solution.</param>
    public Project(string fileName, string displayName)
    {
        this.FileName = fileName;
        this.DisplayName = displayName;
    }

    /// <summary>
    ///     The full path to the project.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    ///     The display name of the project as in the solution.
    /// </summary>
    public string DisplayName { get; }
}