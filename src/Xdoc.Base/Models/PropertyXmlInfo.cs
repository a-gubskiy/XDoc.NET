using System.Xml;
using Xdoc.Abstractions;

namespace Xdoc.Models;

/// <summary>
/// Represents a property in the XML documentation.
/// </summary>
public record PropertyXmlInfo : IPropertyXmlInfo
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Class which the property belongs to.
    /// </summary>
    public IClassXmlInfo Class { get; }

    /// <summary>
    /// Property summary.
    /// </summary>
    public IXmlSummary Summary { get; }

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