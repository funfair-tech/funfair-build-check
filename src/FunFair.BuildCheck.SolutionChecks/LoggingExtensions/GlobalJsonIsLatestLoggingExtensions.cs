using System;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks.LoggingExtensions;

internal static partial class GlobalJsonIsLatestLoggingExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{solutionFileName}: Not checking global.json as DOTNET_CORE_SDK_VERSION is not defined"
    )]
    public static partial void NotCheckingGlobalJson(
        this ILogger<GlobalJsonIsLatest> logger,
        string solutionFileName
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "{solutionFileName}: global.json is using SDK {projectVersion} rather than {dotnetVersion}"
    )]
    public static partial void UsingIncorrectDotNetSdkVersion(
        this ILogger<GlobalJsonIsLatest> logger,
        string solutionFileName,
        string? projectVersion,
        string dotnetVersion
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "{solutionFileName}: global.json does not specify a SDK version"
    )]
    public static partial void DoesNotSpecifyADotNetSdkVersion(
        this ILogger<GlobalJsonIsLatest> logger,
        string solutionFileName
    );

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Error,
        Message = "{solutionFileName}: Failed to read global.json from {file}: {message}"
    )]
    public static partial void FailedToReadGlobalJson(
        this ILogger<GlobalJsonIsLatest> logger,
        string solutionFileName,
        string file,
        string message,
        Exception exception
    );
}
