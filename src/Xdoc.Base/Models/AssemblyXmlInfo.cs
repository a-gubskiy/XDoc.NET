using System.Xml;
using JetBrains.Annotations;

namespace Xdoc.Models;

[PublicAPI]
public record AssemblyXmlInfo
{
    public string Name { get; init; }

    public XmlDocument Xml { get; init; }

    public AssemblyXmlInfo(string name, string xml)
    {
        Name = name;

        Xml = new XmlDocument();
        Xml.LoadXml(xml);
    }
}