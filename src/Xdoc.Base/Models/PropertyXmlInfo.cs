using System.Xml;

namespace Xdoc.Models;

public record PropertyXmlInfo
{
    public string Name { get; init; }
    
    public XmlSummary Summary { get; init; }

    public PropertyXmlInfo(string name, XmlNode xml)
    {
        Name = name;
        Summary = new XmlSummary(xml);
    }
}