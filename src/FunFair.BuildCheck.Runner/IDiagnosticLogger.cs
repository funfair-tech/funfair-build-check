using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner;

public interface IDiagnosticLogger : ILogger
{
    long Errors { get; }

    bool IsErrored { get; }
}