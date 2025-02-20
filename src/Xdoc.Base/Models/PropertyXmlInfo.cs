using System.Xml;

namespace Xdoc.Models;

public record PropertyXmlInfo
{
    public string Name { get; init; }

    public ClassXmlInfo Parent { get; init; }

    public XmlSummary Summary { get; init; }

    public PropertyXmlInfo(string name, ClassXmlInfo parent, XmlNode xml)
    {
        Name = name;
        Parent = parent;
        Summary = new(xml);
    }


    public override string ToString() => Name;
}