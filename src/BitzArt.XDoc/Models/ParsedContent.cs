using System.Collections.Immutable;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents resolver and parsed content from an XML node.
/// </summary>
public record ParsedContent
{
    /// <summary>
    /// List of references.
    /// If references are not found, the dictionary will be empty.
    /// If reference is found, the dictionary will contain the reference name and the parsed content.
    /// If the reference is not resolved, the value will be null.
    /// </summary>
    public required IReadOnlyDictionary<string, ParsedContent?> References { get; init; }

    /// <summary>
    /// XML node from which the content was parsed.
    /// </summary>
    public required XmlNode? Xml { get; init; }

    /// <summary>
    /// Name of the parsed content.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Parent of the parsed content.
    /// </summary>
    public ParsedContent? Parent { get; init; }

    public ParsedContent()
    {
        Parent = null;
        References = ImmutableDictionary<string, ParsedContent?>.Empty;
    }

    public override string ToString() => $"{Name} (References: {References.Count}, Parent: {Parent?.Name ?? "None"})";
}