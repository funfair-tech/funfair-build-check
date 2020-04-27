using System.Xml;

namespace FunFair.BuildCheck
{
    public interface IProjectCheck
    {
        void Check(string projectName, XmlDocument project);
    }
}