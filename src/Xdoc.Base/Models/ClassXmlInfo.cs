using System.Collections.Frozen;
using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo : ISummarized
{
    private readonly Type _type;
    private readonly Dictionary<string, PropertyXmlInfo> _properties;

    public string Name => _type.FullName!;

    public AssemblyXmlInfo Assembly { get; init; }

    public XmlSummary Summary { get; init; }

    public ClassXmlInfo? Parent => Assembly.DocumentStore.GetClassInfo(_type.BaseType);

    internal ClassXmlInfo(Type type, AssemblyXmlInfo assembly, XmlNode xml)
    {
        _type = type;
        _properties = new Dictionary<string, PropertyXmlInfo>();

        Assembly = assembly;
        Summary = new XmlSummary(xml);
    }

    /// <summary>
    /// Fill properties for the class.
    /// </summary>
    /// <param name="documentation"></param>
    internal void FillProperties(IReadOnlyDictionary<string, XmlNode> documentation)
    {
        var typeName = $"P:{_type.FullName}";

        var propertyKeys = documentation.Keys
            .Where(k => k.StartsWith(typeName))
            .ToFrozenSet();

        foreach (var propertyKey in propertyKeys)
        {
            var propertyName = propertyKey[(propertyKey.LastIndexOf('.') + 1)..];

            var node = documentation[propertyKey];
            var propertyXmlInfo = new PropertyXmlInfo(propertyName, node);

            _properties.Add(propertyXmlInfo.Name, propertyXmlInfo);
        }
    }

    /// <summary>
    /// Get information about a property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyXmlInfo? GetPropertyInfo(string propertyName)
    {
        if (_properties.TryGetValue(propertyName, out var propertyXmlInfo))
        {
            return propertyXmlInfo;
        }

        return null;
    }

    public override string ToString() => Name;
}