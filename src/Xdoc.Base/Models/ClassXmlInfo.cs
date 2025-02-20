using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo : ISummarized
{
    private readonly Type _type;
    private readonly IDictionary<string, PropertyXmlInfo> _properties;

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

    /// <summary>
    /// Add properties to the class.
    /// </summary>
    /// <param name="properties"></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void AddProperties(IEnumerable<PropertyXmlInfo> properties)
    {
        foreach (var propertyXmlInfo in properties)
        {
            if (!_properties.TryAdd(propertyXmlInfo.Name, propertyXmlInfo))
            {
                var error = $"Property '{propertyXmlInfo.Name}' already exists in class '{Name}'";

                throw new InvalidOperationException(error);
            }
        }
    }
}