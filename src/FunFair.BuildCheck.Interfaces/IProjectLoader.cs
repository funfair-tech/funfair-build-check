using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectLoader
{
    XmlDocument Load(string path);
}