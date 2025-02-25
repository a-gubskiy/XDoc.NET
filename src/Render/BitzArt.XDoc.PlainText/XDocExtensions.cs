using System.Text;
using System.Xml;
using BitzArt.XDoc;
using Xdoc.Renderer.PlainText;

namespace Xdoc.Renderer.PlaintText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{
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
            sb.AppendLine(Render(parsedContent.OriginalNode));
        }

        if (parsedContent.References.Any())
        {
            sb.AppendLine();
            sb.AppendLine("References: ");
            
            foreach (var reference in parsedContent.References)
            {
                sb.AppendLine(ToPlainText(reference));
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    public static string Render(XmlNode xmlNode)
    {
        var renderer = new XmlRenderer();
        
        return renderer.Render(xmlNode);
        
    }
}