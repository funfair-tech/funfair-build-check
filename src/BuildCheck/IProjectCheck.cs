using System.Xml;

namespace BuildCheck
{
    public interface IProjectCheck
    {
        void Check(string projectName, XmlDocument project);
    }
}