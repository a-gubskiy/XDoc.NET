namespace Xdoc;

public record PropertyXmlInfo
{
    public ClassXmlInfo? Parent { get; set; }
    
    public XmlSummary Summary { get; set; }
}