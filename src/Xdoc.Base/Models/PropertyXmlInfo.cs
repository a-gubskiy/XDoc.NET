using System.Xml;

namespace Xdoc.Models;

public record PropertyXmlInfo : ISummarized
{
    public string Name { get; init; }

    public ClassXmlInfo Class { get; internal set; }

    public XmlSummary Summary { get; init; }

    internal PropertyXmlInfo(string name, ClassXmlInfo classInfo, XmlNode xml)
    {
        Name = name;
        Summary = new XmlSummary(xml);
        Class = classInfo;
    }

    public override string ToString() => Name;
}