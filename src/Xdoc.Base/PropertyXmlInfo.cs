namespace Xdoc;

public record PropertyXmlInfo
{
    public ClassXmlInfo? Parent { get; init; }
    
    public string Name { get; init; } = "";

    public XmlSummary? Summary { get; init; }
}