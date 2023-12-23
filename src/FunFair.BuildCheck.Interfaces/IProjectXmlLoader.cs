using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectXmlLoader
{
    ValueTask<XmlDocument> LoadAsync(string path, CancellationToken cancellationToken);
}