using System.Xml;
using Xdoc.Abstractions;

namespace Xdoc.Models;

/// <summary>
/// Represents a summary in the XML documentation.
/// </summary>
public record XmlSummary : IXmlSummary
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