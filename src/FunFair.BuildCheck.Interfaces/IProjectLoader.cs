using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

/// <summary>
///     Loads projects.
/// </summary>
public interface IProjectLoader
{
    /// <summary>
    ///     Loads the named project.
    /// </summary>
    /// <param name="path">The project.</param>
    /// <returns>The loaded project</returns>
    XmlDocument Load(string path);
}