using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="MethodInfo"/>
/// </summary>
public sealed class MethodDocumentation : TypeMemberDocumentation<MethodInfo>
{
    internal MethodDocumentation(XDoc source, TypeDocumentation declaringTypeDocumentation, MethodInfo method, XmlNode node)
        : base(source, declaringTypeDocumentation, method, node)
    {
    }
}
