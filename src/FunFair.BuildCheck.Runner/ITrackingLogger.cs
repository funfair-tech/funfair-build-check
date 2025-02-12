using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner;

public interface ITrackingLogger : ILogger
{
    long Errors { get; }

    bool IsErrored { get; }
}
