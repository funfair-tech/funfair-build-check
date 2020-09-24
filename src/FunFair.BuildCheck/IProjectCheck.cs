using System.Xml;

namespace FunFair.BuildCheck
{
    /// <summary>
    ///     A Project check.
    /// </summary>
    public interface IProjectCheck
    {
        /// <summary>
        ///     Checks the project for issues.
        /// </summary>
        /// <param name="projectName">The project to check.</param>
        /// <param name="project">the loaded XML dom of the project file.</param>
        void Check(string projectName, XmlDocument project);
    }
}