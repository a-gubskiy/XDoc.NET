using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo
{
    private readonly Type _type;
    private readonly XmlNode _xml;
    
    public string Name => _type.FullName!;

    public ClassXmlInfo(Type type, XmlNode xml)
    {
        _type = type;
        _xml = xml;
    }

    /// <summary>
    /// Get information about a property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyXmlInfo? GetPropertyInfo(string propertyName)
    {
        return GetPropertyInfo(_type, propertyName);
    }

    private PropertyXmlInfo? GetPropertyInfo(Type type, string propertyName)
    {
        var xpath = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";
        var node = _xml.SelectSingleNode(xpath);

        if (node != null)
        {
            var inheritdoc = node.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetPropertyInfo(type.BaseType, propertyName);
            }

            return new PropertyXmlInfo(propertyName, node);
        }

        return null;
    }
    
    public override string ToString() => Name;
}