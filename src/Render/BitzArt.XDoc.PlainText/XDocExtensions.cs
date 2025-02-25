using System.Text;
using BitzArt.XDoc;
using Xdoc.Renderer.PlainText;

namespace Xdoc.Renderer.PlaintText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{
    private static readonly XmlRenderer Renderer = new XmlRenderer();
    
    /// <summary>
    /// Get full representation of the type documentation in plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this TypeDocumentation documentation)
    {
        var parsedContent = documentation.ParsedContent;

        return ToPlainText(parsedContent);
    }

    /// <summary>
    /// Get full representation of the property documentation in plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this PropertyDocumentation documentation)
    {
        var parsedContent = documentation.ParsedContent;

        return ToPlainText(parsedContent);
    }

    private static string ToPlainText(ParsedContent parsedContent)
    {
        
        var sb = new StringBuilder();

        if (parsedContent.Parent != null)
        {
            sb.AppendLine(ToPlainText(parsedContent.Parent));
        }
        else
        {
            var text = Renderer.Render(parsedContent.Xml);
            
            sb.AppendLine(text);
        }

        if (parsedContent.References.Any())
        {
            sb.AppendLine();
            sb.AppendLine("References: ");

            foreach (var reference in parsedContent.References)
            {
                sb.AppendLine($" – {reference.Type.Name}");
                sb.AppendLine(ToPlainText(reference));
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}