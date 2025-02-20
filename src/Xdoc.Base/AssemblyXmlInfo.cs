using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Xdoc;

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