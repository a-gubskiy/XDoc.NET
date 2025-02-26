using System.Collections.Immutable;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents resolver and parsed content from an XML node.
/// </summary>
public record ParsedContent
{
    /// <summary>
    /// List of resolved references to other parsed content.
    /// </summary>
    public required IReadOnlyCollection<ParsedContent> ResolvedReferences { get; init; }

    /// <summary>
    /// List of references declared in the XML node.
    /// </summary>
    public required IReadOnlyCollection<string> DeclaredReferences { get; init; }

    /// <summary>
    /// XML node from which the content was parsed.
    /// </summary>
    public required XmlNode? Xml { get; init; }

    public required string Name { get; init; }

    /// <summary>
    /// Parent of the parsed content.
    /// </summary>
    public ParsedContent? Parent { get; init; }

    public ParsedContent()
    {
        Parent = null;
        ResolvedReferences = ImmutableList<ParsedContent>.Empty;
    }

    public override string ToString() => $"{Name} (References: {ResolvedReferences.Count}, Parent: {Parent?.Name ?? "None"})";
}