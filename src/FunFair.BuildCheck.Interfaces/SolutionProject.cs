using System.Diagnostics;

namespace FunFair.BuildCheck.Interfaces;

[DebuggerDisplay("{DisplayName}: {FileName}")]
public sealed record SolutionProject(string FileName, string DisplayName);
