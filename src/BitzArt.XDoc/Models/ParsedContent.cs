using System.Collections.Immutable;
using System.Xml;

namespace BitzArt.XDoc;

public record ParsedContent
{
    public IReadOnlyCollection<ParsedContent> References { get; init; }

    public XmlNode OriginalNode { get; init; }

    public ParsedContent? Parent { get; init; }

    public ParsedContent()
    {
        Parent = null;
        References = ImmutableList<ParsedContent>.Empty;
    }
}