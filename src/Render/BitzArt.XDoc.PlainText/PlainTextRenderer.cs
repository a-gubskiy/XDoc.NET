using System.Reflection;
using System.Text;
using System.Xml;

namespace BitzArt.XDoc;

public interface IDcoumentationRenderer
{
    string Render(DocumentationElement documentation);
}

/// <summary>
/// Lightweight XML renderer that converts XML documentation to plain text.
/// This implementation, can only render the text content of the XML nodes, but not resolve and render references.
/// </summary>
public class PlainTextRenderer : IDcoumentationRenderer
{
    public string Render(DocumentationElement documentation)
    {
        throw new NotImplementedException();
    }
}