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

        return new PlainTextRenderer(documentation).Render();
    }
 
    /// <summary>
    /// The documentation instance to be rendered by this renderer.
    /// This field is initialized in the constructor and remains readonly afterward.
    /// </summary>
    protected readonly MemberDocumentation Documentation;

    protected PlainTextRenderer(MemberDocumentation documentation)
    {
        Documentation = documentation;
    }

    /// <summary>
    /// Renders the current documentation to plain text.
    /// </summary>
    /// <returns>A normalized plain text representation of the documentation.</returns>
    protected string Render() => Normalize(Render(Documentation.Node));

    /// <summary>
    /// Renders the content of an XML node to plain text.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private string Render(XmlNode? node) => node switch
    {
        null => string.Empty,
        XmlText textNode => RenderTextNode(textNode),
        XmlElement element => RenderXmlElement(element),
        _ => node.InnerText
    };

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

        foreach (XmlNode child in element.ChildNodes)
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
    protected virtual string RenderReference(XmlElement element)
    {
        var documentationReference = Documentation.GetReference(element);

        if (documentationReference == null)
        {
            return string.Empty;
        }

        return RenderDocumentationReference(documentationReference);
    }

    /// <summary>
    /// Renders a documentation reference.
    /// </summary>
    /// <param name="documentationReference"></param>
    /// <returns></returns>
    private static string RenderDocumentationReference(DocumentationReference documentationReference)
    {
        if (documentationReference.Target == null)
        {
            return string.Empty;
        }
        
        if (documentationReference.RequirementNode.Name == "inheritdoc")
        {
            return Render(documentationReference.Target); 
        }

        var text = documentationReference.Target switch
        {
            TypeDocumentation typeDocumentation => typeDocumentation.Type.Name,
            FieldDocumentation fieldDocumentation => fieldDocumentation.MemberName,
            PropertyDocumentation propertyDocumentation => propertyDocumentation.MemberName,
            MethodDocumentation methodDocumentation => methodDocumentation.MemberName,
            _ => string.Empty
        };

        return text;

        // var renderer = new PlainTextRenderer(documentationReference.Target);
        //
        // return renderer.Render();
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