namespace BitzArt.XDoc;

/// <summary>
/// Configuration options for controlling XML documentation rendering behavior.
/// </summary>
public record RendererOptions
{
    /// <summary>
    /// Indicating whether the content should be forced to render as a single line.
    /// When set to true, any line breaks in the content will be removed or replaced.
    /// When set to false, the content will maintain its original line structure.
    /// </summary>
    public bool Trim { get; init; } = true;

    /// <summary>
    /// Whether to remove the namespace from the type names.
    /// </summary>
    public bool RemoveNamespace { get; init; } = true;
}