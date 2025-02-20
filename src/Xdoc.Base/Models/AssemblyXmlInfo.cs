using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Xml;
using JetBrains.Annotations;
using Xdoc.Abstractions;

namespace Xdoc.Models;

/// <summary>
/// Represents an assembly in the XML documentation.
/// </summary>
[PublicAPI]
public record AssemblyXmlInfo : IAssemblyXmlInfo
{
    public string Name { get; init; }

    internal IDocumentStore DocumentStore { get; }

    private readonly IDictionary<Type, ClassXmlInfo> _classes;

    /// <summary>
    /// Initialize a new instance of <see cref="AssemblyXmlInfo"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="xml"></param>
    /// <param name="documentStore"></param>
    internal AssemblyXmlInfo(string name, string xml, IDocumentStore documentStore)
    {
        _classes = new Dictionary<Type, ClassXmlInfo>();

        DocumentStore = documentStore;
        Name = name;

        var documentation = LoadXmlDocumentation(xml);
        var types = GetTypeNames(documentation);

        foreach (var type in types)
        {
            var classXmlInfo = new ClassXmlInfo(type, this, documentation);

            _classes.Add(type, classXmlInfo);
        }
    }

    /// <summary>
    /// Get a list of type names from the XML documentation.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    private IReadOnlyCollection<Type> GetTypeNames(IReadOnlyDictionary<string, XmlNode> documentation)
    {
        var exactlyTypeNames = documentation.Keys
            .Where(k => k.StartsWith("T:"))
            .Select(o => o.Replace("T:", ""))
            .ToList();

        var typeNamesFromDefinedMembers = documentation.Keys
            .Where(k => k.StartsWith("M:") || k.StartsWith("P:"))
            .Select(o => o.Replace("M:", "").Replace("P:", ""))
            .Select(o => o[..o.LastIndexOf('.')])
            .ToList();

        var typeNames = new[] { exactlyTypeNames, typeNamesFromDefinedMembers }
            .SelectMany(o => o)
            .Distinct()
            .ToFrozenSet();

        // Create a list of types
        var types = typeNames
            .Select(o => Type.GetType($"{o}, {Name}"))
            .Where(o => o != null)
            .Select(o => o!)
            .ToFrozenSet();

        return types;
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

    /// <summary>
    /// Try to find information about a class.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ClassXmlInfo? GetClassInfo(Type type)
    {
        if (_classes.TryGetValue(type, out var classXmlInfo))
        {
            return classXmlInfo;
        }

        var typeAssemblyName = type.Assembly.GetName();

        if (typeAssemblyName.Name != Name)
        {
            return DocumentStore.GetClassInfo(type);
        }

        return null;
    }

    /// <summary>
    /// Get a string representation of the of <see cref="AssemblyXmlInfo"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;
}