using System.Xml;

namespace Xdoc;

public record XmlSummary
{
    public XmlNode Xml { get; }
    
    public string Value { get; init; } = "";

    public XmlSummary(XmlNode node)
    {
        Xml = node;
        Value = node.InnerXml;
    }
}