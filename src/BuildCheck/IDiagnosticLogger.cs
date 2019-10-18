using Microsoft.Extensions.Logging;

namespace BuildCheck
{
    public interface IDiagnosticLogger : ILogger
    {
        long Errors { get; }

        bool IsErrored { get; }
    }
}