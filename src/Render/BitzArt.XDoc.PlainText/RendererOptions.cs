namespace BitzArt.XDoc.PlainText;

/// <summary>
/// Configuration options for controlling XML documentation rendering behavior.
/// </summary>
public record RendererOptions
{
    /// <summary>
    /// Whether to remove the namespace from the type names.
    /// </summary>
    public bool RemoveNamespace { get; set; } = true;
}