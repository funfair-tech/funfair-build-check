using System.Diagnostics;
using FunFair.BuildCheck.SolutionChecks.Models;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

[DebuggerDisplay("Info={Info}")]
public sealed class GlobalJsonLoadedResult : GlobalJsonLoadResult
{
    public GlobalJsonLoadedResult(GlobalJsonInfo info)
    {
        this.Info = info;
    }

    public GlobalJsonInfo Info { get; }
}
