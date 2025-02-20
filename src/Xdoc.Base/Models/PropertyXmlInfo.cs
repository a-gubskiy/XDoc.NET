using System.Xml;

namespace Xdoc.Models;

public record PropertyXmlInfo
{
    private readonly XmlNode _xml;

    public string Name { get; init; }

    public PropertyXmlInfo(string name, XmlNode xml)
    {
        _xml = xml;
        
        Name = name;
    }

    public XmlNode GetXml() => _xml;

    public override string ToString() => Name;
}