using System.Xml;
using JetBrains.Annotations;

namespace Xdoc.Models;

[PublicAPI]
public record AssemblyXmlInfo
{
    public string Name { get; init; }
    
    public XmlDocument Xml { get; set; }

    public AssemblyXmlInfo(string name, string xml)
    {
        Name = name;

        Xml = new XmlDocument();
        Xml.LoadXml(xml);
    }

    public ClassXmlInfo? GetClassInfo(Type type)
    {
        var xpath = $"/doc/members/member[@name='T:{type.FullName}']";
        var node = Xml.SelectSingleNode(xpath);

        if (node != null)
        {
            var inheritdoc = node.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetClassInfo(type.BaseType);
            }

            return new ClassXmlInfo(type, this, node);
        }

        return null;
    }

    public override string ToString() => Name;
}