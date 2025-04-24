using System.Collections.Generic;
using System.Diagnostics;

namespace FunFair.BuildCheck.ProjectChecks.Models;

[DebuggerDisplay("{Id}")]
public readonly record struct PackageReference(
    string Id,
    IReadOnlyDictionary<string, string> Attributes
);
