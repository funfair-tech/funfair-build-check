using System.Diagnostics;
using System.Xml;

namespace FunFair.BuildCheck.Interfaces;

[DebuggerDisplay("Project: {Name}, Folder: {Folder}")]
public readonly record struct ProjectContext(string Name, string Folder, XmlDocument CsProjXml);
