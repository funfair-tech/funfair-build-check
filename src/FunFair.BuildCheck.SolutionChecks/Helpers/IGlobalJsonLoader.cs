using System.Threading;
using System.Threading.Tasks;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

public interface IGlobalJsonLoader
{
    ValueTask<GlobalJsonLoadResult> LoadAsync(string solutionFileName, CancellationToken cancellationToken);

    void Clear();
}
