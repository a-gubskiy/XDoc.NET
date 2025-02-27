using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="PropertyInfo"/>
/// </summary>
public sealed class PropertyDocumentation : TypeMemberDocumentation<PropertyInfo>
{
    internal PropertyDocumentation(XDoc source, TypeDocumentation parentNode, PropertyInfo property, XmlNode node)
        : base(source, parentNode, property, node)
    {
    }
}
