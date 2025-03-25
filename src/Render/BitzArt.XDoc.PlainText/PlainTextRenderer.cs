using System.Text;
using System.Xml;

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
    private readonly XDoc _xdoc;
    private readonly RendererOptions _options;

    public PlainTextRenderer(XDoc xdoc)
        : this(xdoc, new RendererOptions())
    {
    }

    public PlainTextRenderer(XDoc xdoc, RendererOptions options)
    {
        _xdoc = xdoc;
        _options = options;
    }

    /// <inheritdoc />
    public string Render(DocumentationElement documentation)
    {
        return Normalize(Render(documentation?.Node));
    }

    /// <summary>
    /// Normalize the input string by removing extra empty lines and trimming each line.
    /// </summary>
    private string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var lines = input
            .Split('\n')
            .Select(line => line.Trim())
            .Where(o => !string.IsNullOrWhiteSpace(o))
            .ToList();

        var separator = _options.ForceSingleLine ? " " : "\n";

        return string.Join(separator, lines);
    }

    private string Render(XmlNode? node) => node switch
    {
        null => string.Empty,
        XmlText textNode => RenderTextNode(textNode),
        XmlElement element => RenderXmlElement(element),
        _ => node.InnerText
    };

    /// <summary>
    /// Renders the content of an XML text node to plain text.
    /// </summary>
    /// <param name="textNode">The XML text node to render.</param>
    /// <returns>The plain text representation of the XML text node.</returns>
    private string RenderTextNode(XmlText textNode)
    {
        return textNode.Value ?? string.Empty;
    }

    /// <summary>
    /// Renders the content of an XML element to plain text, including handling child nodes and references.
    /// </summary>
    /// <param name="element">The XML element to render.</param>
    /// <returns>The plain text representation of the XML element.</returns>
    private string RenderXmlElement(XmlElement element)
    {
        var builder = new StringBuilder();

        var crefAttribute = element.Attributes["cref"];

        if (element.Name == "inheritdoc" && crefAttribute == null)
        {
            // Direct inheritance
            return RenderDirectInheritance(element);
        }
        else if (element.Name == "inheritdoc" && crefAttribute != null)
        {
            // Inheritance with reference
            return RenderInheritanceWithReference(element, crefAttribute);
        }
        else if (crefAttribute != null)
        {
            // Reference
            return RenderReference(element, crefAttribute);
        }

        foreach (XmlNode child in element.ChildNodes)
        {
            builder.Append(Render(child));
        }

        return builder.ToString();
    }

    private string RenderReference(XmlElement element, XmlAttribute crefAttribute)
    {
        var cref = new Cref(crefAttribute.Value);

        if (cref.IsMember)
        {
            return $"{cref.ShortType}.{cref.Member}";
        }

        return cref.ShortType;
    }

    private string RenderInheritanceWithReference(XmlElement element, XmlAttribute crefAttribute)
    {
        var cref = new Cref(crefAttribute.Value);

        if (cref.IsMember)
        {
            return $"{cref.ShortType}.{cref.Member}";
        }

        return cref.ShortType;
    }

    private string RenderDirectInheritance(XmlElement element)
    {
        //We can't now get the parent node of the current node, so we can't get the parent node of the parent node

        return string.Empty;
        // _xdoc.Get()
    }
}