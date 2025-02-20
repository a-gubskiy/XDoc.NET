using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo : ISummarized
{
    public AssemblyXmlInfo Assembly { get; }

    private readonly Type _type;
    private readonly IDictionary<string, PropertyXmlInfo> _properties;
    
    private ClassXmlInfo? _parent;

    public string Name => _type.FullName!;

    public ClassXmlInfo? Parent
    {
        get
        {
            if (_parent == null && _type.BaseType != null && _type.BaseType != typeof(object))
            {
                _parent = Assembly.GetClassInfo(_type.BaseType);
            }

            return _parent;
        }
    }

    public XmlSummary Summary { get; init; }

    internal ClassXmlInfo(Type type, AssemblyXmlInfo assembly, Dictionary<string, PropertyXmlInfo> properties, XmlNode xml)
    {
        _type = type;
        _properties = properties;
        
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
}