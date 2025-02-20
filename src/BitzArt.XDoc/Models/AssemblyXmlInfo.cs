using JetBrains.Annotations;
using System.Collections.Frozen;
using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents an assembly in the XML documentation.
/// </summary>
[PublicAPI]
public class AssemblyXmlInfo : XmlInfo<Assembly>
{
    private readonly Dictionary<Type, TypeXmlInfo> _data;

    internal AssemblyXmlInfo(XDoc xdoc, Assembly assembly) : base(xdoc, assembly)
    {
        var filePath = Path.ChangeExtension(assembly.Location, "xml");

        XmlDocumentationFileNotFoundException.ThrowIfFileNotFound(filePath);

        var content = File.ReadAllText(filePath);

        _data = [];

        var documentation = LoadXmlDocumentation(content);
        var types = GetTypes(documentation);

        foreach (var type in types)
        {
            var classXmlInfo = new TypeXmlInfo(type, this, documentation);

            _data.Add(type, classXmlInfo);
        }
    }

    private IReadOnlyCollection<Type> GetTypes(IReadOnlyDictionary<string, XmlNode> documentation)
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

    private static IReadOnlyDictionary<string, XmlNode> LoadXmlDocumentation(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

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
    public TypeXmlInfo? GetData(Type type)
    {
        if (_data.TryGetValue(type, out var classXmlInfo))
        {
            return classXmlInfo;
        }

        var typeAssemblyName = type.Assembly.GetName();

        if (typeAssemblyName.Name != Name)
        {
            return _xdoc.GetClassInfo(type);
        }

        return null;
    }

    /// <inheritdoc/>
    public override string ToString() => Assembly.GetName().Name!;
}