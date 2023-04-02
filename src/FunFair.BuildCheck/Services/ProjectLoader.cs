using System;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using NonBlocking;

namespace FunFair.BuildCheck.Services;

public sealed class ProjectLoader : IProjectLoader
{
    private readonly ConcurrentDictionary<string, XmlDocument> _projects;

    public ProjectLoader()
    {
        this._projects = new(StringComparer.Ordinal);
    }

    public XmlDocument Load(string path)
    {
        if (this._projects.TryGetValue(key: path, out XmlDocument? doc))
        {
            return doc;
        }

        return this._projects.GetOrAdd(key: path, LoadProject(path));
    }

    private static XmlDocument LoadProject(string path)
    {
        XmlDocument document = new();
        document.Load(path);

        return document;
    }
}