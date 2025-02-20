using System.Xml;

namespace Xdoc;

public record ClassXmlInfo
{
    public string Name { get; init; }
    
    public XmlSummary Summary { get; init; }

    public ClassXmlInfo(string name, XmlNode xml)
    {
        Name = name;
        Summary = new XmlSummary(xml);
    }
}