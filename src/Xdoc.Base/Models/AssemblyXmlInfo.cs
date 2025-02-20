using System.Xml;
using JetBrains.Annotations;

namespace Xdoc.Models;

[PublicAPI]
public record AssemblyXmlInfo
{
    public string Name { get; init; }

    private readonly IDocumentStore _documentStore;
    private readonly IDictionary<Type, ClassXmlInfo> _classes;

    public AssemblyXmlInfo(string name, string xml, IDocumentStore documentStore)
    {
        _documentStore = documentStore;

        Name = name;

        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml); // TODO: Parse to dictionary with Type as Key

        IReadOnlyCollection<Type> classes = GetClasses(xml); // FullName

        foreach (var @class in classes)
        {
            var type = Type.GetType(@class);

            IDictionary<string, PropertyXmlInfo> properties = GetProperties(xml, @class);
            new ClassXmlInfo(type, this, properties, node);
        }
    }

    public ClassXmlInfo? GetClassInfo(Type type)
    {
        if (_classes.TryGetValue(type, out var classXmlInfo))
        {
            return classXmlInfo;
        }

        return _documentStore.GetClassInfo(type);
    }

    public override string ToString() => Name;
}