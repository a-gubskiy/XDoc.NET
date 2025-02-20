using System.Xml;

namespace Xdoc;

public record ClassXmlInfo
{
    public string? Name { get; }
    
    public XmlNode Xml { get; }

    public ClassXmlInfo(string? name, XmlNode xml)
    {
        Name = name;
        Xml = xml;
    }
}