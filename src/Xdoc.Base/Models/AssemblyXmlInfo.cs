using System.Collections.Frozen;
using System.Xml;
using JetBrains.Annotations;

namespace Xdoc.Models;

[PublicAPI]
public record AssemblyXmlInfo
{
    public string Name { get; init; }

    internal IDocumentStore DocumentStore { get; }

    private readonly IDictionary<Type, ClassXmlInfo> _classes;

    internal AssemblyXmlInfo(string name, string xml, IDocumentStore documentStore)
    {
        _classes = new Dictionary<Type, ClassXmlInfo>();

        DocumentStore = documentStore;
        Name = name;

        // T:TestAssembly.A.ClassA
        // P:TestAssembly.A.ClassA.Name
        // M:TestAssembly.A.ClassA.GetName

        var documentation = LoadXmlDocumentation(xml);

        // Extract all type names
        var typeNames = documentation.Keys
            .Where(k => k.StartsWith("T:"))
            .Select(o => o.Replace("T:", ""))
            .ToFrozenSet();

        // Create a list of types
        var types = typeNames
            .Select(o => Type.GetType($"{o}, {Name}"))
            .Where(o => o != null)
            .Select(o => o!)
            .ToFrozenSet();

        foreach (var type in types)
        {
            var classXmlInfo = new ClassXmlInfo(type, this, documentation[$"T:{type.FullName}"]);

            classXmlInfo.FillProperties(documentation);
            
            _classes.Add(type, classXmlInfo);
        }
    }

   

    /// <summary>
    /// Load the XML documentation into a collection of XML nodes.
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    private static IReadOnlyDictionary<string, XmlNode> LoadXmlDocumentation(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml); // TODO: Parse to dictionary with Type as Key

        var xpath = "/doc/members/member";
        var nodes = xmlDocument.SelectNodes(xpath)?.Cast<XmlNode>() ?? [];


        var documentation = nodes
            .Where(o => o.Attributes != null && o.Attributes!["name"] != null)
            .ToFrozenDictionary(o => o.Attributes!["name"]!.Value, o => o);

        return documentation;
    }

    public ClassXmlInfo? GetClassInfo(Type type)
    {
        if (_classes.TryGetValue(type, out var classXmlInfo))
        {
            return classXmlInfo;
        }

        return DocumentStore.GetClassInfo(type);
    }

    public override string ToString() => Name;
}