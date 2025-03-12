using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="FieldInfo"/>
/// </summary>
public sealed class FieldDocumentation : TypeMemberDocumentation<FieldInfo>
{
    internal FieldDocumentation(XDoc source, TypeDocumentation declaringTypeDocumentation, FieldInfo field, XmlNode node)
        : base(source, declaringTypeDocumentation, field, node)
    {
    }
}