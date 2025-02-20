using System.Xml;

namespace Xdoc.Models;

public record PropertyXmlInfo : ISummarized
{
    public string Name { get; init; }

    public ClassXmlInfo Class { get; init; }

    public XmlSummary Summary { get; init; }

    internal PropertyXmlInfo(string name, ClassXmlInfo @class, XmlNode xml)
    {
        Name = name;
        Class = @class;
        Summary = new XmlSummary(xml);
    }

    public override string ToString() => Name;
}