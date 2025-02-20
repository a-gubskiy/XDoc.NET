using System.Xml;

namespace Xdoc.Models;

public record XmlSummary
{
    public XmlNode? Xml { get; }
    
    public XmlSummary(XmlNode? node)
    {
        Xml = node;
    }
}