using FunFair.BuildCheck.ProjectChecks.Settings;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Helpers;

internal sealed class CapturingLogger
    : CapturingLoggerBase,
        ILogger<SimplePropertyProjectCheckBase>,
        ILogger<MustNotDefinePropertyProjectCheckBase>;

internal sealed class CapturingLogger<T> : CapturingLoggerBase, ILogger<T>;
