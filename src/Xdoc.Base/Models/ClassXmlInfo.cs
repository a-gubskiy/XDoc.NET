using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo
{
    public AssemblyXmlInfo Parent { get; }
    
    private readonly Type _type;
    
    public string Name => _type.FullName!;
    
    public XmlSummary Summary { get; init; }

    public ClassXmlInfo(Type type, AssemblyXmlInfo parent, XmlNode xml)
    {
        _type = type;
        
        Parent = parent;
        Summary = new XmlSummary(xml);
    }

    /// <summary>
    /// Get information about a property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyXmlInfo? GetPropertyInfo(string propertyName)
    {
        var xmlNode = GetPropertyInfo(_type, propertyName);

        if (xmlNode == null)
        {
            return null;
        }
        
        return new PropertyXmlInfo(propertyName, this, xmlNode);
    }

    private XmlNode? GetPropertyInfo(Type type, string propertyName)
    {
        var xpath = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";
        var node = Parent.Xml.SelectSingleNode(xpath);

        if (node != null)
        {
            var inheritdoc = node.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetPropertyInfo(type.BaseType, propertyName);
            }

            return node;
        }

        return null;
    }
    
    public override string ToString() => Name;
}