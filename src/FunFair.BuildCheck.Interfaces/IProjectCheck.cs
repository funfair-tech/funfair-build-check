using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectCheck
{
    ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken);
}
