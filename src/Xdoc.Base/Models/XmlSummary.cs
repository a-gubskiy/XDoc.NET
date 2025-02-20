using System.Xml;

namespace Xdoc.Models;

/// <summary>
/// Represents a summary in the XML documentation.
/// </summary>
public record XmlSummary
{
    public XmlNode? Xml { get; }
    
    public XmlSummary(XmlNode? node)
    {
        Xml = node;
    }
}