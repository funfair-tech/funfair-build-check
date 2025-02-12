using System;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks.LoggingExtensions;

internal static partial class GlobalJsonMustSpecifyCorrectRollForwardPolicyLoggingExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "{solutionFileName}: global.json is using SDK rollForward policy of {projectPolicy} rather than {expectedPolicy}")]
    public static partial void UsingIncorrectRollForwardPolicy(
        this ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> logger,
        string solutionFileName,
        string projectPolicy,
        string expectedPolicy
    );

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{solutionFileName}: global.json does not specify a SDK rollForward policy")]
    public static partial void DoesNotSpecifyADotNetSdkRollForwardPolicy(this ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> logger, string solutionFileName);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "{solutionFileName}: Failed to read global.json from {file}: {message}")]
    public static partial void FailedToReadGlobalJson(this ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> logger, string solutionFileName, string file, string message, Exception exception);
}
