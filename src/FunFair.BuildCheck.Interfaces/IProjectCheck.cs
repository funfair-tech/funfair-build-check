using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectCheck
{
    void Check(string projectName, string projectFolder, XmlDocument project);
}