using System.Collections.Generic;
using System.Diagnostics;

namespace FunFair.BuildCheck.ProjectChecks.Models;

[DebuggerDisplay("{PackageId}")]
public readonly record struct PackageReference(string PackageId, IReadOnlyDictionary<string, string> Attributes);