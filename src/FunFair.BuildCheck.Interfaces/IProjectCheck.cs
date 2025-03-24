using System.Threading;
using System.Threading.Tasks;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectCheck
{
    ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken);
}
