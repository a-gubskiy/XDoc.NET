using System.Xml;

namespace Xdoc.Models;

/// <summary>
/// Represents a property in the XML documentation.
/// </summary>
public record PropertyXmlInfo : ISummarized
{
    public string Name { get; init; }

    public ClassXmlInfo Class { get; internal set; }

    public XmlSummary Summary { get; init; }

    /// <summary>
    /// Initialize a new instance of <see cref="PropertyXmlInfo"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="classInfo"></param>
    /// <param name="xml"></param>
    internal PropertyXmlInfo(string name, ClassXmlInfo classInfo, XmlNode xml)
    {
        Name = name;
        Summary = new XmlSummary(xml);
        Class = classInfo;
    }

    /// <summary>
    /// Get a string representation of the of <see cref="PropertyXmlInfo"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;
}