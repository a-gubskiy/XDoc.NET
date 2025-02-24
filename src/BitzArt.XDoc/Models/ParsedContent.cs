using System.Collections.Immutable;

namespace BitzArt.XDoc;

public record ParsedContent
{
    public IReadOnlyCollection<ParsedContent> Crefs { get; init; }
    
    public ParsedContent? Parent { get; init; }
    
    public ParsedContent()
    {
        Parent = null;
        Crefs = ImmutableList<ParsedContent>.Empty;
    }
}