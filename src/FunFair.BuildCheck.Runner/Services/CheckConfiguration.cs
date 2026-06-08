using System.Diagnostics;
using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Runner.Services;

[DebuggerDisplay("PreReleaseBuild={PreReleaseBuild}, AllowPackageVersionMismatch={AllowPackageVersionMismatch}")]
public sealed record CheckConfiguration(bool PreReleaseBuild, bool AllowPackageVersionMismatch) : ICheckConfiguration;
