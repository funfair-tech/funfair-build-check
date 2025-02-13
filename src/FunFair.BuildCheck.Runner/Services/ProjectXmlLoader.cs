using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using NonBlocking;

namespace FunFair.BuildCheck.Runner.Services;

public sealed class ProjectXmlLoader : IProjectXmlLoader
{
    private readonly ConcurrentDictionary<string, WeakReference<XmlDocument>> _projects;

    public ProjectXmlLoader()
    {
        this._projects = new(StringComparer.Ordinal);
    }

    public ValueTask<XmlDocument> LoadAsync(string path, CancellationToken cancellationToken)
    {
        return this.TryGetFromCache(path: path, out XmlDocument? document)
            ? ValueTask.FromResult(document)
            : this.LoadPotentiallyCachedAsync(path: path, cancellationToken: cancellationToken);
    }

    private bool TryGetFromCache(string path, [NotNullWhen(true)] out XmlDocument? document)
    {
        if (this._projects.TryGetValue(key: path, out WeakReference<XmlDocument>? weakDocument))
        {
            return weakDocument.TryGetTarget(out document);
        }

        document = null;

        return false;
    }

    private async ValueTask<XmlDocument> LoadPotentiallyCachedAsync(
        string path,
        CancellationToken cancellationToken
    )
    {
        XmlDocument doc = await LoadProjectAsync(path: path, cancellationToken: cancellationToken);

        WeakReference<XmlDocument> weakDocument = this._projects.GetOrAdd(
            key: path,
            new WeakReference<XmlDocument>(doc)
        );

        if (weakDocument.TryGetTarget(out XmlDocument? document))
        {
            return document;
        }

        weakDocument.SetTarget(doc);

        return doc;
    }

    private static async ValueTask<XmlDocument> LoadProjectAsync(
        string path,
        CancellationToken cancellationToken
    )
    {
        string content = await File.ReadAllTextAsync(
            path: path,
            encoding: Encoding.UTF8,
            cancellationToken: cancellationToken
        );
        XmlDocument doc = new();

        doc.LoadXml(content);

        return doc;
    }
}
