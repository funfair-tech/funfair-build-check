using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Tests.Helpers;

[DebuggerDisplay("[{Level}] {Message}")]
internal readonly record struct CapturedLogEntry(LogLevel Level, EventId EventId, string Message);
