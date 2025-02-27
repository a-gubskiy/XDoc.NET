using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="MethodInfo"/>
/// </summary>
public sealed class MethodDocumentation : TypeMemberDocumentation<MethodInfo>
{
    internal MethodDocumentation(XDoc source, TypeDocumentation parentNode, MethodInfo method, XmlNode node)
        : base(source, parentNode, method, node)
    {
    }
}
