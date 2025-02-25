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
    public static string ToPlainText(this TypeDocumentation? documentation) =>
        ToPlainText(documentation?.ParsedContent);

    /// <summary>
    /// Get full representation of the property documentation in plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this PropertyDocumentation? documentation) =>
        ToPlainText(documentation?.ParsedContent);

    /// <summary>
    /// Converts a <see cref="ParsedContent"/> object to its plain text representation,
    /// including parent documentation and references.
    /// </summary>
    /// <param name="parsedContent">
    /// The parsed content to convert, containing XML documentation.
    /// </param>
    /// <returns>A string containing the plain text representation of the documentation.</returns>
    private static string ToPlainText(ParsedContent? parsedContent)
    {
        if (parsedContent == null)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        if (parsedContent.Parent != null)
        {
            builder.AppendLine(ToPlainText(parsedContent.Parent));
        }
        else
        {
            builder.AppendLine(Renderer.Render(parsedContent.Xml));
        }

        if (parsedContent.References.Any())
        {
            builder.Append(RenderReference(parsedContent));
        }

        return builder.ToString().Trim();
    }

    private static string RenderReference(ParsedContent parsedContent)
    {
        var builder = new StringBuilder();

        builder.AppendLine();
        builder.AppendLine("References: ");

        foreach (var reference in parsedContent.References)
        {
            builder.AppendLine($" – {reference.Name}");
            builder.AppendLine(ToPlainText(reference));
            builder.AppendLine();
        }

        return builder.ToString();
    }
}