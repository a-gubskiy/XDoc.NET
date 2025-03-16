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

        var result = Render(documentation, documentation.Node);

        return result;
    }

    /// <summary>
    /// Renders the content of an XML node to plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    private static string Render(MemberDocumentation documentation, XmlNode? node)
    {
        return Normalize(node switch
        {
            null => string.Empty,
            XmlText textNode => RenderTextNode(textNode),
            XmlElement element => RenderXmlElement(documentation, element),
            _ => node.InnerText
        });
    }

    /// <summary>
    /// Normalize the input string by removing extra empty lines and trimming each line.
    /// </summary>
    private static string Normalize(string input)
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
    /// <param name="documentation"></param>
    /// <param name="element">The XML element to render.</param>
    /// <returns>The plain text representation of the XML element.</returns>
    private static string RenderXmlElement(MemberDocumentation documentation, XmlElement element)
    {
        var builder = new StringBuilder();

        if (element.Attributes["cref"] != null || element.Name == "inheritdoc")
        {
            return RenderReference(documentation, element);
        }

        var childNodes = element.ChildNodes.Cast<XmlNode>().ToList();

        foreach (var child in childNodes)
        {
            builder.Append(Render(documentation, child));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Render a see/seealso reference.
    /// </summary>
    /// <param name="documentation"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    private static string RenderReference(MemberDocumentation documentation, XmlElement element)
    {
        var documentationReference = documentation.GetReference(element);

        if (documentationReference == null)
        {
            return string.Empty;
        }

        return Render(documentationReference.Target, documentationReference.Target.Node);
    }

    /// <summary>
    /// Renders the content of an XML text node to plain text.
    /// </summary>
    /// <param name="textNode">The XML text node to render.</param>
    /// <returns>The plain text representation of the XML text node.</returns>
    private static string RenderTextNode(XmlText textNode)
    {
        return textNode.Value ?? string.Empty;
    }
}