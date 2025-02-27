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
    private readonly IReadOnlyDictionary<string, ParsedContent?> _references;

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

    internal ParsedContent(IReadOnlyDictionary<string, ParsedContent?> references)
    {
        Parent = null;

        _references = references;
    }

    public override string ToString()
        => $"{Name} (References: {_references.Count}, Parent: {Parent?.Name ?? "None"})";

    public IReadOnlyDictionary<string, ParsedContent?> GetReferences()
    {
        return _references;
    }
}