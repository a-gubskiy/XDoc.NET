namespace BitzArt.XDoc;

/// <summary>
/// Interface for rendering documentation elements.
/// </summary>
public interface IDocumentationRenderer
{
    /// <summary>
    /// Renders the provided documentation element to a string.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    string Render(DocumentationElement documentation);
}

/// <summary>
/// Lightweight renderer that converts XML documentation to plain text.
/// This implementation, can only render the text content of the XML nodes, but not resolve and render references.
/// </summary>
public class PlainTextRenderer : IDocumentationRenderer
{
    /// <inheritdoc />
    public string Render(DocumentationElement documentation)
    {
        throw new NotImplementedException();
    }
}