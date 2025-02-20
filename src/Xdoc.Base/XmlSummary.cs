using System.Xml;

namespace Xdoc;

public record XmlSummary
{
    public XmlNode Node { get; }
    
    public string Value { get; init; } = "";

    public XmlSummary(XmlNode node)
    {
        Node = node;
        Value = node.InnerXml;
    }
}