using System.Xml;

namespace Xdoc.Models;

/// <summary>
/// Represents a summary in the XML documentation.
/// </summary>
public record XmlSummary
{
    public XmlNode? Xml { get; }
    
    /// <summary>
    /// Initialize a new instance of <see cref="XmlSummary"/>.
    /// </summary>
    /// <param name="node"></param>
    public XmlSummary(XmlNode? node)
    {
        Xml = node;
    }
}