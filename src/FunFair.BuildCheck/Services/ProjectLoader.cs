using System.Collections.Concurrent;
using System.Xml;
using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Services
{
    /// <summary>
    ///     Loads a project.
    /// </summary>
    public sealed class ProjectLoader : IProjectLoader
    {
        private readonly ConcurrentDictionary<string, XmlDocument> _projects;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ProjectLoader()
        {
            this._projects = new ConcurrentDictionary<string, XmlDocument>();
        }

        /// <inheritdoc />
        public XmlDocument Load(string path)
        {
            return this._projects.GetOrAdd(key: path, valueFactory: LoadProject);
        }

        private static XmlDocument LoadProject(string path)
        {
            XmlDocument document = new();
            document.Load(path);

            return document;
        }
    }
}