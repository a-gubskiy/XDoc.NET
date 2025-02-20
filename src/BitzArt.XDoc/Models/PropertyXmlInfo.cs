using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents a property in the XML documentation.
/// </summary>
public record PropertyXmlInfo
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Class which the property belongs to.
    /// </summary>
    public TypeXmlInfo Type { get; }

    /// <summary>
    /// Property summary.
    /// </summary>
    public XmlSummary Summary { get; }

    /// <summary>
    /// Initialize a new instance of <see cref="PropertyXmlInfo"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="classInfo"></param>
    /// <param name="xml"></param>
    internal PropertyXmlInfo(string name, TypeXmlInfo classInfo, XmlNode xml)
    {
        Name = name;
        Summary = new XmlSummary(xml);
        Type = classInfo;
    }

    /// <summary>
    /// Get a string representation of the of <see cref="PropertyXmlInfo"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;
}