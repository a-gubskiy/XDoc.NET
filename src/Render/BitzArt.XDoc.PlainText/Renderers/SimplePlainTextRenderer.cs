using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Lightweight XML renderer that converts XML documentation to plain text.
/// This implementation, can only render the text content of the XML nodes, but not resolve and render references.
/// </summary>
public class SimplePlainTextRenderer : PlainTextRenderer
{
    /// <summary>
    /// Converts an XML documentation node to the plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public new static string Render(MemberDocumentation? documentation)
    {
        if (documentation == null)
        {
            return string.Empty;
        }

        return new SimplePlainTextRenderer(documentation).Render();
    }

    private SimplePlainTextRenderer(MemberDocumentation documentation)
        : base(documentation)
    {
    }

    /// <inheritdoc/>
    protected override string RenderReference(XmlElement element)
    {
        var documentationReference = Documentation.GetReference(element);

        if (documentationReference == null)
        {
            return string.Empty;
        }

        var cref = documentationReference.Cref;

        if (string.IsNullOrWhiteSpace(cref))
        {
            return string.Empty;
        }

        var prefix = cref[..2];
        var lastIndexOf = cref.LastIndexOf('.');

        if (prefix is "T:")
        {
            return cref.Substring(lastIndexOf + 1, cref.Length - lastIndexOf - 1);
        }

        if (prefix is "M:" or "P:" or "F:")
        {
            var type = cref.Substring(2, lastIndexOf - 2);
            var method = cref.Substring(lastIndexOf + 1, cref.Length - lastIndexOf - 1);

            var typeLastIndexOf = type.LastIndexOf('.');
            type = type.Substring(typeLastIndexOf + 1, type.Length - typeLastIndexOf - 1);

            return $"{type}.{method}";
        }

        return string.Empty;
    }
}