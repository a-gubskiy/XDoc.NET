using System.Text;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Lightweight XML renderer that converts XML documentation to plain text.
/// This implementation, can only render the text content of the XML nodes, but not resolve and render references.
/// </summary>
public class PlainTextRenderer
{
    /// <summary>
    /// Converts an XML documentation node to the plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string Render(MemberDocumentation? documentation)
    {
        if (documentation == null)
        {
            return string.Empty;
        }

        var renderer = new PlainTextRenderer(documentation);

        return renderer.Render();
    }

    private readonly MemberDocumentation _documentation;

    private PlainTextRenderer(MemberDocumentation documentation)
    {
        _documentation = documentation;
    }

    private string Render()
    {
        var result = Render(_documentation.Node);

        return result;
    }

    /// <summary>
    /// Renders the content of an XML node to plain text.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private string Render(XmlNode? node)
    {
        return Normalize(node switch
        {
            null => string.Empty,
            XmlText textNode => RenderTextNode(textNode),
            XmlElement element => RenderXmlElement(element),
            _ => node.InnerText
        });
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

        var result = string.Join("\n", lines);

        return result;
    }

    /// <summary>
    /// Renders the content of an XML element to plain text, including handling child nodes and references.
    /// </summary>
    /// <param name="element">The XML element to render.</param>
    /// <returns>The plain text representation of the XML element.</returns>
    private string RenderXmlElement(XmlElement element)
    {
        var builder = new StringBuilder();

        if (element.Attributes["cref"] != null || element.Name == "inheritdoc")
        {
            return RenderReference(element);
        }

        var childNodes = element.ChildNodes.Cast<XmlNode>().ToList();

        foreach (var child in childNodes)
        {
            builder.Append(Render(child));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Render a see/seealso reference.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private string RenderReference(XmlElement element)
    {
        var documentationReference = _documentation.GetReference(element);

        if (documentationReference == null)
        {
            return string.Empty;
        }

        var renderer = new PlainTextRenderer(documentationReference.Target);

        return renderer.Render();
    }

    /// <summary>
    /// Renders the content of an XML text node to plain text.
    /// </summary>
    /// <param name="textNode">The XML text node to render.</param>
    /// <returns>The plain text representation of the XML text node.</returns>
    private string RenderTextNode(XmlText textNode)
    {
        return textNode.Value ?? string.Empty;
    }
}