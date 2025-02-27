using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents resolver and parsed content from an XML node.
/// </summary>
public record ParsedContent
{
    public MemberDocumentation Documentation { get; private set; }

    /// <summary>
    /// List of references.
    /// If references are not found, the dictionary will be empty.
    /// If reference is found, the dictionary will contain the reference name and the parsed content.
    /// If the reference is not resolved, the value will be null.
    /// </summary>
    private readonly IReadOnlyDictionary<string, ParsedContent?> _references;

    private ParsedContent? _inheritedContent;

    /// <summary>
    /// Inherited documentation content.
    /// </summary>
    public ParsedContent? InheritedContent => _inheritedContent ??= ResolveInheritedContent();

    private ParsedContent? ResolveInheritedContent()
    {
        return ParsedContentResolver.GetInheritedContent(Documentation);
    }


    internal ParsedContent(MemberDocumentation documentation, IReadOnlyDictionary<string, ParsedContent?> references)
    {
        Documentation = documentation;
        
        _references = references;
    }

    public override string ToString()
        => $"(References: {_references.Count}";

    public IReadOnlyDictionary<string, ParsedContent?> GetReferences()
    {
        return _references;
    }
}