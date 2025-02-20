using System.Xml;

namespace Xdoc;

public record PropertyXmlInfo
{
    public string Name { get; }
    public XmlNode Xml { get; }

    public PropertyXmlInfo(string name, XmlNode xml)
    {
        Name = name;
        Xml = xml;
    }
}