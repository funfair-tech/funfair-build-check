using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck;

public interface IDiagnosticLogger : ILogger
{
    long Errors { get; }

    bool IsErrored { get; }
}