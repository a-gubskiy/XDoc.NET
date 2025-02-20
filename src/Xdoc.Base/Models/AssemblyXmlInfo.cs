using System.Xml;
using JetBrains.Annotations;

namespace Xdoc.Models;

[PublicAPI]
public record AssemblyXmlInfo
{
    public readonly XmlDocument _xml;
    
    public string Name { get; init; }

    public AssemblyXmlInfo(string name, string xml)
    {
        Name = name;

        _xml = new XmlDocument();
        _xml.LoadXml(xml);
    }

    public ClassXmlInfo? GetClassInfo(Type type)
    {
        var xpath = $"/doc/members/member[@name='T:{type.FullName}']";
        var node = _xml.SelectSingleNode(xpath);

        if (node != null)
        {
            var inheritdoc = node.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetClassInfo(type.BaseType);
            }

            return new ClassXmlInfo(type, node);
        }

        return null;
    }

    public override string ToString() => Name;
}