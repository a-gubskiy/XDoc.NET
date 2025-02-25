using System.Collections.Immutable;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents resolver and parsed content from an XML node.
/// </summary>
public record ParsedContent
{
    /// <summary>
    /// List of references to other parsed content.
    /// </summary>
    public IReadOnlyCollection<ParsedContent> References { get; init; }

    /// <summary>
    /// XML node from which the content was parsed.
    /// </summary>
    public required XmlNode? Xml { get; init; }

    public required string Name { get; set; }

    /// <summary>
    /// Parent of the parsed content.
    /// </summary>
    public ParsedContent? Parent { get; init; }

    public ParsedContent()
    {
        Parent = null;
        References = ImmutableList<ParsedContent>.Empty;
    }

    public override string ToString() => $"{Name} (References: {References.Count}, Parent: {Parent?.Name ?? "None"})";
}