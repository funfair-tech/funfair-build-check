using System;
using System.Diagnostics;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

[DebuggerDisplay("File={File}, Exception={Exception}")]
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
