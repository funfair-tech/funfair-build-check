using System.Diagnostics;

namespace FunFair.BuildCheck.SolutionChecks.Models;

[DebuggerDisplay("SdkVersion={SdkVersion}, RollForward={RollForward}, AllowPrerelease={AllowPrerelease}")]
public sealed record GlobalJsonInfo(string? SdkVersion, string? RollForward, bool? AllowPrerelease);
