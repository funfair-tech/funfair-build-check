using System.Threading;
using System.Threading.Tasks;

namespace FunFair.BuildCheck.Interfaces;

public interface ISolutionCheck
{
    ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken);
}