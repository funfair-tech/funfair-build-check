using System;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

public sealed class GlobalJsonReadFailedResult : GlobalJsonLoadResult
{
    public GlobalJsonReadFailedResult(string file, Exception exception)
    {
        this.File = file;
        this.Exception = exception;
    }

    public string File { get; }

    public Exception Exception { get; }
}
